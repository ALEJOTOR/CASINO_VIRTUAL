using BLL.Wompi;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLL
{
    public class WompiServicio
    {
        private readonly IWompiHttpClient _httpClient;
        private readonly IWompiSignatureService _signatureService;
        private readonly string _publicKey;
        private readonly string _privateKey;
        private readonly string _apiUrl;

        // Payouts (Pagos a Terceros) - LLAVES SEPARADAS
        private readonly string _payoutsApiKey;
        private readonly string _payoutsUserPrincipalId;
        private readonly string _payoutsAccountId;
        private readonly string _payoutsBaseUrl;
        private readonly bool _payoutsSandbox;

        private static List<BancoWompi> _bancosCacheados;
        private static DateTime _tiempoCache = DateTime.MinValue;
        private static readonly TimeSpan _duracionCache = TimeSpan.FromHours(24);

        // Cache de bancos para Payouts (UUID-based, diferente al cache de cobros)
        private static List<WompiPayoutBank> _bancosPagosTercerosCacheados;
        private static DateTime _tiempoCachePagos = DateTime.MinValue;

        public WompiServicio()
            : this(
                new WompiHttpClient(),
                new WompiSignatureService(ConfigurationManager.AppSettings["Wompi.LlaveIntegridad"] ?? ""),
                ConfigurationManager.AppSettings["Wompi.LlavePublica"] ?? "",
                ConfigurationManager.AppSettings["Wompi.LlavePrivada"] ?? "",
                ConfigurationManager.AppSettings["Wompi.Ambiente"] ?? "sandbox",
                ConfigurationManager.AppSettings["Wompi.Payouts.ApiKey"] ?? "",
                ConfigurationManager.AppSettings["Wompi.Payouts.UserPrincipalId"] ?? "",
                ConfigurationManager.AppSettings["Wompi.Payouts.AccountId"] ?? "",
                ConfigurationManager.AppSettings["Wompi.Payouts.BaseUrl"] ?? "https://sandbox.wompi.co/api/v1"
            )
        {
        }

        public WompiServicio(
            IWompiHttpClient httpClient,
            IWompiSignatureService signatureService,
            string publicKey,
            string privateKey,
            string ambiente,
            string payoutsApiKey = "",
            string payoutsUserPrincipalId = "",
            string payoutsAccountId = "",
            string payoutsBaseUrl = "https://sandbox.wompi.co/api/v1")
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _signatureService = signatureService ?? throw new ArgumentNullException(nameof(signatureService));
            _publicKey = publicKey ?? "";
            _privateKey = privateKey ?? "";

            bool esProduccion = ambiente?.ToLower() == "produccion" || ambiente?.ToLower() == "production";
            _apiUrl = esProduccion ? "https://api.wompi.co/v1" : "https://sandbox.wompi.co/v1";

            // Payouts
            _payoutsApiKey = payoutsApiKey ?? "";
            _payoutsUserPrincipalId = payoutsUserPrincipalId ?? "";
            _payoutsAccountId = payoutsAccountId ?? "";
            _payoutsBaseUrl = payoutsBaseUrl ?? "https://sandbox.wompi.co/api/v1";
            _payoutsSandbox = _payoutsBaseUrl.Contains("sandbox");
        }

        public bool TieneLlavesValidas()
        {
            return !string.IsNullOrEmpty(_publicKey)
                && !string.IsNullOrEmpty(_privateKey);
        }

        public bool TieneLlavesPayoutsValidas()
        {
            return !string.IsNullOrEmpty(_payoutsApiKey)
                && !string.IsNullOrEmpty(_payoutsUserPrincipalId)
                && !string.IsNullOrEmpty(_payoutsAccountId);
        }

        public async Task<List<BancoWompi>> ObtenerBancosAsync()
        {
            if (_bancosCacheados != null && DateTime.Now - _tiempoCache < _duracionCache)
                return _bancosCacheados;

            try
            {
                string endpoint = $"{_apiUrl}/merchants/{_publicKey}/payment_methods/financial_institutions";

                using (var response = await _httpClient.GetAsync(endpoint, _publicKey))
                {
                    if (!response.IsSuccessStatusCode)
                        return BancosWompiDefecto.ObtenerListaDefecto();

                    string body = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<WompiFinancialInstitutionsResponse>(body,
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower });

                    if (result?.Data != null && result.Data.Count > 0)
                    {
                        _bancosCacheados = result.Data
                            .Where(i => i.IsActive)
                            .Select(i => new BancoWompi { Id = i.Id, Nombre = i.Name })
                            .ToList();

                        _tiempoCache = DateTime.Now;
                        return _bancosCacheados;
                    }
                }
            }
            catch
            {
            }

            return BancosWompiDefecto.ObtenerListaDefecto();
        }

        /// <summary>
        /// Obtiene lista de bancos para Payouts (Pagos a Terceros) desde Wompi.
        /// Los UUIDs retornados se usan en CrearPagoTerceroAsync.
        /// Cache: 24 horas (igual que ObtenerBancosAsync)
        /// </summary>
        public async Task<List<WompiPayoutBank>> ObtenerBancosPayoutsAsync()
        {
            if (_bancosPagosTercerosCacheados != null && DateTime.Now - _tiempoCachePagos < _duracionCache)
                return _bancosPagosTercerosCacheados;

            if (!TieneLlavesPayoutsValidas())
            {
                return new List<WompiPayoutBank>();
            }

            try
            {
                string endpoint = $"{_payoutsBaseUrl}/payouts/banks";

                using (var request = new HttpRequestMessage(HttpMethod.Get, endpoint))
                {
                    request.Headers.Add("x-api-key", _payoutsApiKey);
                    request.Headers.Add("user-principal-id", _payoutsUserPrincipalId);

                    using (var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) })
                    using (var response = await httpClient.SendAsync(request))
                    {
                        string body = await response.Content.ReadAsStringAsync();

                        if (!response.IsSuccessStatusCode)
                        {
                            return new List<WompiPayoutBank>();
                        }

                        var result = JsonSerializer.Deserialize<WompiPayoutBanksResponse>(body,
                            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower });

                        if (result?.Data != null && result.Data.Count > 0)
                        {
                            _bancosPagosTercerosCacheados = result.Data.Where(b => b.IsActive).ToList();
                            _tiempoCachePagos = DateTime.Now;
                            return _bancosPagosTercerosCacheados;
                        }
                    }
                }
            }
            catch
            {
            }

            return new List<WompiPayoutBank>();
        }

        // Mapeo manual entre códigos de cobros y nombres de bancos para mejor búsqueda en Payouts
        private static readonly Dictionary<string, string> _mapeoCodigosANombres = new Dictionary<string, string>
        {
            // Bancos principales de Colombia (códigos ASOBANCARIA)
            { "1052", "BANCOLOMBIA" },           // Bancolombia
            { "1040", "BANCO COMERCIAL COLOMBIANO" },  // BCC
            { "1002", "BANCO DE OCCIDENTE" },   // Banco de Occidente
            { "1006", "BANCO GANADERO" },       // Banco Ganadero (Agrario)
            { "1012", "BANCO SANTANDER" },      // Banco Santander
            { "1023", "BANCO CORP. DE INVERSIONES" },  // CORFICOLOMBIANA
            { "1051", "BANCO SABADELL" },       // Banco Sabadell
            { "1065", "BANCO BARCLAYS" },       // Banco Barclays
            { "1066", "BANCO FALABELLA" },      // Banco Falabella
            { "1069", "BANCO CAJA SOCIAL" },    // Banco Caja Social
            { "1083", "BANCO VALORES" },        // Banco Valores
            { "1087", "BANCOOMEVA" },           // Bancoomeva
            { "1090", "BANCO FINAMEX" },        // Banco Finamex
            { "1094", "BANCO DE LA REPUBLICA" }, // Banco de la República
            { "1097", "BANCO COMPARTAMOS" },    // Banco Compartamos
            { "1112", "BMONEX" },                // Monex
            { "1113", "BMULTIVA" },              // Multiva
            { "1116", "ING" },                   // ING
            { "1124", "DEUTSCHE" },              // Deutsche Bank
            { "1125", "LEHMAN BROTHERS" },       // Lehman Brothers
            { "1128", "HSBC" },                  // HSBC
            { "1130", "BANCO JP MORGAN" },       // JP Morgan
            { "1143", "REFORMA" },               // Banco Reforma
            { "1151", "MICROFINANZAS" },         // Banco Microfinanzas
            { "1152", "UNIBANCO" },              // Unibanco
            { "1154", "DAVIVIENDA" },            // Davivienda
            { "1155", "BANCO W" },               // Banco W
            { "1156", "BANCO BOGOTA" },          // Banco Bogotá
            { "1157", "BANCO PAGATODO" },        // Banco Pagatodo
            { "1158", "BANCO PICHINCHA" },       // Banco Pichincha
            { "1201", "SCOTIABANK" },            // Scotiabank
            { "1202", "BBVA" },                  // BBVA Bancomer
            { "1203", "BANCO AV VILLAS" },       // Banco Av Villas
            { "1205", "BANCO ACTINVER" },        // Actinver
            { "1206", "INTERCAM" },              // Intercam
            { "1207", "BANCAFE" },               // Bancafé
            { "1208", "PROFESIONALES" },         // Banco de los Profesionales
            { "1209", "SOFIMEX" },               // Sofimex
            { "1210", "CONAVI" },                // Conavi
            { "1211", "BANREGIO" },              // Banregio
            { "1213", "INVEX" },                 // Invex
            { "1214", "AFIRME" },                // Banco Afirme
            { "1215", "INBURSA" },               // Inbursa
            { "1219", "MIFELICITY" },            // Mifelicity
            { "1220", "MIZUHO BANK" },           // Mizuho Bank
            { "1221", "BANCO MONEX" },           // Banco Monex
            { "1222", "BMULTIVA" },              // Banco Multiva
            { "1224", "ICBC" },                  // ICBC
            { "1226", "PAGMEX" },                // Pagmex
            { "1227", "CREDIT SUISSE" },         // Credit Suisse
            { "1228", "BAYSER" },                // Bayser
            { "1229", "BARCLAYS" },              // Barclays
            { "1230", "COMPARTAMOS BANCO" },     // Compartamos Banco
        };

        /// <summary>
        /// Obtiene el UUID de un banco (necesario para Payouts) a partir del código de banco de cobros.
        /// Busca en la lista de bancos de Payouts coincidencia por Code, Name o comparación flexible.
        /// También intenta usar BancoNombre si está disponible en el objeto DatosBancarios.
        /// </summary>
        public async Task<string> ObtenerBankIdPayoutPorCodigoAsync(string codigoBanco, string nombreBanco = null)
        {
            if (string.IsNullOrWhiteSpace(codigoBanco) && string.IsNullOrWhiteSpace(nombreBanco))
                return null;

            var bancos = await ObtenerBancosPayoutsAsync();
            if (bancos == null || bancos.Count == 0)
                return null;

            string codigoNormalizado = codigoBanco?.ToUpper().Trim() ?? "";
            string nombreNormalizado = nombreBanco?.ToUpper().Trim() ?? "";

            // Primero intentar match exacto en Code con el código
            var banco = bancos.FirstOrDefault(b => 
                b.Code != null && b.Code.Equals(codigoNormalizado, StringComparison.OrdinalIgnoreCase));

            if (banco != null)
                return banco.Id;

            // Si el nombre del banco no viene, intentar mapeo manual del código
            if (string.IsNullOrEmpty(nombreNormalizado) && _mapeoCodigosANombres.TryGetValue(codigoNormalizado, out var nombreMapeado))
            {
                nombreNormalizado = nombreMapeado;
            }

            // Intentar match en Name con el nombre del banco
            if (!string.IsNullOrEmpty(nombreNormalizado))
            {
                banco = bancos.FirstOrDefault(b => 
                    b.Name != null && b.Name.Equals(nombreNormalizado, StringComparison.OrdinalIgnoreCase));

                if (banco != null)
                    return banco.Id;

                // Intentar match parcial (Contains) con el nombre
                banco = bancos.FirstOrDefault(b => 
                    b.Name != null && b.Name.ToUpper().Contains(nombreNormalizado));

                if (banco != null)
                    return banco.Id;
            }

            // Intentar match en Name con el código (ej: "BANCOLOMBIA" contiene "1052"? no, pero puede ser por otro mapeo)
            banco = bancos.FirstOrDefault(b => 
                b.Name != null && b.Name.Equals(codigoNormalizado, StringComparison.OrdinalIgnoreCase));

            if (banco != null)
                return banco.Id;

            // Intentar match parcial (Contains) con el código
            if (!string.IsNullOrEmpty(codigoNormalizado))
            {
                banco = bancos.FirstOrDefault(b => 
                    b.Name != null && b.Name.ToUpper().Contains(codigoNormalizado));

                if (banco != null)
                    return banco.Id;
            }

            // Si no se encuentra, retornar null
            return null;
        }

        /// <summary>
        /// Obtiene lista de todos los bancos disponibles en Payouts (para debug/configuración).
        /// Útil para mapear correctamente códigos de bancos entre sistemas.
        /// </summary>
        public async Task<List<WompiPayoutBank>> ObtenerBancosPayoutsParaDebugAsync()
        {
            var bancos = await ObtenerBancosPayoutsAsync();
            return bancos ?? new List<WompiPayoutBank>();
        }

        public async Task<(bool ok, string link, string idLink, string error)> CrearLinkPagoAsync(decimal monto, string referencia)
        {
            try
            {
                long centavos = (long)(monto * 100);
                string firma = _signatureService.GenerarFirma(referencia, centavos, "COP");

                    var payload = new
                    {
                        amount_in_cents = centavos,
                        currency = "COP",
                        name = "Deposito Casino Virtual",
                        description = $"Deposito por ${monto:N2} - Ref: {referencia}",
                        single_use = true,
                        collect_shipping = false,
                        reference = referencia,
                        expires_at = DateTime.UtcNow.AddHours(48).ToString("o"),
                        signature = new { integrity = firma }
                    };

                string endpoint = $"{_apiUrl}/payment_links";

                using (var response = await _httpClient.PostAsync(endpoint, _privateKey, payload))
                {
                    string body = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        string error = ParsearError(body, (int)response.StatusCode);
                        return (false, null, null, error);
                    }

                    var result = JsonSerializer.Deserialize<WompiLinkResponse>(body,
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower });

                    string idLink = result?.Data?.Id;
                    string linkUrl = result?.Data?.Url;

                    if (string.IsNullOrEmpty(linkUrl) && !string.IsNullOrEmpty(idLink))
                    {
                        linkUrl = $"https://checkout.wompi.co/p" +
                            $"?public-key={_publicKey}" +
                            $"&currency=COP" +
                            $"&amount-in-cents={centavos}" +
                            $"&reference={Uri.EscapeDataString(referencia)}" +
                            $"&signature%3Aintegrity={firma}";
                    }

                    if (string.IsNullOrEmpty(linkUrl))
                    {
                        return (false, null, null, $"Wompi no devolvio link. Response: {body}");
                    }

                    return (true, linkUrl, idLink, null);
                }
            }
            catch (TaskCanceledException)
            {
                return (false, null, null, "La solicitud a Wompi excedio el tiempo de espera.");
            }
            catch (HttpRequestException ex)
            {
                return (false, null, null, $"Error de conexion con Wompi: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, null, null, $"Error inesperado al crear link: {ex.Message}");
            }
        }

        public async Task<(bool ok, List<WompiTransaction> transacciones, string error)> ConsultarTransaccionesLinkAsync(string idLink)
        {
            try
            {
                string endpoint = $"{_apiUrl}/payment_links/{idLink}";

                using (var response = await _httpClient.GetAsync(endpoint, _privateKey))
                {
                    string body = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        string error = ParsearError(body, (int)response.StatusCode);
                        return (false, null, error);
                    }

                    var result = JsonSerializer.Deserialize<WompiLinkResponse>(body,
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower });

                    return (true, result?.Data?.Transactions ?? new List<WompiTransaction>(), null);
                }
            }
            catch (TaskCanceledException)
            {
                return (false, null, "Tiempo de espera agotado al consultar link.");
            }
            catch (HttpRequestException ex)
            {
                return (false, null, $"Error de conexion: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, null, $"Error al consultar transacciones: {ex.Message}");
            }
        }

        public async Task<(bool ok, WompiTransactionData transaccion, string error)> ConsultarTransaccionAsync(string idTransaccion)
        {
            try
            {
                string endpoint = $"{_apiUrl}/transactions/{idTransaccion}";

                using (var response = await _httpClient.GetAsync(endpoint, _privateKey))
                {
                    string body = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        string error = ParsearError(body, (int)response.StatusCode);
                        return (false, null, error);
                    }

                    var result = JsonSerializer.Deserialize<WompiTransactionResponse>(body,
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower });

                    return (true, result?.Data, null);
                }
            }
            catch (TaskCanceledException)
            {
                return (false, null, "Tiempo de espera agotado al consultar transaccion.");
            }
            catch (HttpRequestException ex)
            {
                return (false, null, $"Error de conexion: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, null, $"Error al consultar transaccion: {ex.Message}");
            }
        }

        /// <summary>
        /// Crea un pago a tercero (retiro) mediante la API de Wompi Payouts.
        /// - Obtiene el UUID del banco destino (diferente al ID de cobros)
        /// - Construye la solicitud de Payout
        /// - En sandbox, agrega transactionStatus="APPROVED" para simular éxito
        /// - Retorna el payoutId si es exitoso
        /// </summary>
        public async Task<(bool ok, string payoutId, string error)> CrearPagoTerceroAsync(
            decimal monto, DatosBancarios datos, string referencia, string emailUsuario = null)
        {
            if (!TieneLlavesPayoutsValidas())
            {
                return (false, null, "Las llaves de Wompi Payouts no están configuradas.");
            }

            try
            {
                // Obtener UUID del banco (es diferente al ID de cobros)
                string bankIdUuid = await ObtenerBankIdPayoutPorCodigoAsync(datos.BancoId, datos.BancoNombre);
                if (string.IsNullOrEmpty(bankIdUuid))
                {
                    return (false, null, $"Banco '{datos.BancoNombre ?? datos.BancoId}' no encontrado en sistemas de Payouts de Wompi.");
                }

                long montoEnCentavos = (long)(monto * 100);

                // Construir la solicitud de Payout
                var payoutRequest = new WompiPayoutRequest
                {
                    Reference = referencia,
                    AccountId = _payoutsAccountId,
                    PaymentType = "OTHERS",  // Para retiros de casino
                    Transactions = new List<WompiPayoutTransaction>
                    {
                        new WompiPayoutTransaction
                        {
                            LegalIdType = datos.TipoDoc,
                            LegalId = datos.NumeroDoc,
                            BankId = bankIdUuid,  // UUID del banco, no el código
                            AccountType = datos.TipoCuenta,  // "AHORROS" o "CORRIENTE"
                            AccountNumber = datos.NumeroCuenta,
                            Name = datos.NombreTitular,
                            Email = emailUsuario ?? "casino@example.com",
                            Amount = montoEnCentavos,
                            Reference = $"TRX-{referencia}"
                        }
                    }
                };

                // En sandbox, simular aprobación agregando transactionStatus
                if (_payoutsSandbox)
                {
                    payoutRequest.TransactionStatus = "APPROVED";
                }

                // Serializar el payload
                var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };
                string jsonPayload = JsonSerializer.Serialize(payoutRequest, jsonOptions);

                // Realizar POST a Wompi Payouts
                string endpoint = $"{_payoutsBaseUrl}/payouts";

                using (var request = new HttpRequestMessage(HttpMethod.Post, endpoint))
                {
                    request.Headers.Add("x-api-key", _payoutsApiKey);
                    request.Headers.Add("user-principal-id", _payoutsUserPrincipalId);
                    request.Headers.Add("idempotency-key", referencia);  // Prevenir duplicados
                    request.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

                    using (var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) })
                    using (var response = await httpClient.SendAsync(request))
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        if (!response.IsSuccessStatusCode)
                        {
                            string error = ParsearErrorPayouts(responseBody, (int)response.StatusCode);
                            return (false, null, error);
                        }

                        var result = JsonSerializer.Deserialize<WompiPayoutResponse>(responseBody, jsonOptions);
                        if (result?.Data?.Id != null)
                        {
                            return (true, result.Data.Id, null);
                        }

                        return (false, null, "Wompi no retornó un ID de payout válido.");
                    }
                }
            }
            catch (TaskCanceledException)
            {
                return (false, null, "Timeout: Mujcho tiempo esperando respuesta de Wompi Payouts.");
            }
            catch (HttpRequestException ex)
            {
                return (false, null, $"Error de conexión con Wompi Payouts: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, null, $"Error inesperado creando payout en Wompi: {ex.Message}");
            }
        }

        public async Task<(bool ok, List<WompiTransactionData> transacciones, string error)> ConsultarTransaccionesPorReferenciaAsync(string referencia)
        {
            try
            {
                string endpoint = $"{_apiUrl}/transactions?reference={Uri.EscapeDataString(referencia)}";

                using (var response = await _httpClient.GetAsync(endpoint, _privateKey))
                {
                    string body = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        string error = ParsearError(body, (int)response.StatusCode);
                        return (false, null, error);
                    }

                    var result = JsonSerializer.Deserialize<WompiTransactionListResponse>(body,
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower });

                    return (true, result?.Data ?? new List<WompiTransactionData>(), null);
                }
            }
            catch (TaskCanceledException)
            {
                return (false, null, "Tiempo de espera agotado al consultar transacciones.");
            }
            catch (HttpRequestException ex)
            {
                return (false, null, $"Error de conexion: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, null, $"Error al consultar transacciones: {ex.Message}");
            }
        }

        /// <summary>
        /// Consulta el estado de un payout previo.
        /// Retorna: (ok, estado, detalle de error, error general)
        /// Estados posibles: TOTAL_PAYMENT, PARTIAL_PAYMENT, FAILED, PROCESSING
        /// </summary>
        public async Task<(bool ok, string estado, string errorDetalle, string error)> ConsultarEstadoPayoutAsync(string payoutId)
        {
            if (!TieneLlavesPayoutsValidas())
            {
                return (false, null, null, "Las llaves de Wompi Payouts no están configuradas.");
            }

            if (string.IsNullOrWhiteSpace(payoutId))
            {
                return (false, null, null, "ID de payout inválido.");
            }

            try
            {
                string endpoint = $"{_payoutsBaseUrl}/payouts/{payoutId}";

                using (var request = new HttpRequestMessage(HttpMethod.Get, endpoint))
                {
                    request.Headers.Add("x-api-key", _payoutsApiKey);
                    request.Headers.Add("user-principal-id", _payoutsUserPrincipalId);

                    using (var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) })
                    using (var response = await httpClient.SendAsync(request))
                    {
                        string body = await response.Content.ReadAsStringAsync();

                        if (!response.IsSuccessStatusCode)
                        {
                            string error = ParsearErrorPayouts(body, (int)response.StatusCode);
                            return (false, null, null, error);
                        }

                        var result = JsonSerializer.Deserialize<WompiPayoutDetailResponse>(body,
                            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower });

                        if (result?.Data != null)
                        {
                            string errorDetalle = null;

                            // Si hay transacciones fallidas, obtener el detalle del error
                            if (result.Data.Transactions != null && result.Data.Transactions.Count > 0)
                            {
                                var failed = result.Data.Transactions.FirstOrDefault(t => 
                                    t.Status == "FAILED" && t.FailureReason != null);
                                
                                if (failed != null)
                                {
                                    errorDetalle = $"{failed.FailureReason.Code}: {failed.FailureReason.Message}";
                                }
                            }

                            return (true, result.Data.Status, errorDetalle, null);
                        }

                        return (false, null, null, "Wompi no retornó datos válidos.");
                    }
                }
            }
            catch (TaskCanceledException)
            {
                return (false, null, null, "Timeout consultando estado de payout.");
            }
            catch (HttpRequestException ex)
            {
                return (false, null, null, $"Error de conexión: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, null, null, $"Error consultando payout: {ex.Message}");
            }
        }

        private static string ParsearError(string body, int statusCode)
        {
            try
            {
                var err = JsonSerializer.Deserialize<WompiErrorResponse>(body,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower });

                if (err?.Errors != null && err.Errors.Count > 0)
                {
                    return string.Join(" | ", err.Errors.Select(e => $"[{e.Type}] {e.Message ?? e.Reason}"));
                }
            }
            catch
            {
            }

            switch (statusCode)
            {
                case 401: return "Error de autenticacion con Wompi. Verifique las llaves API.";
                case 404: return $"Error HTTP 404 de Wompi. URL o recurso no encontrado. Response: {body}";
                case 422: return $"Datos invalidos enviados a Wompi. Response: {body}";
                case 429: return "Demasiadas solicitudes a Wompi. Espere e intente de nuevo.";
                default: return $"Error HTTP {statusCode} de Wompi.";
            }
        }

        private static string ParsearErrorPayouts(string body, int statusCode)
        {
            try
            {
                // Intentar parsear como error estándar de Wompi
                var err = JsonSerializer.Deserialize<WompiErrorResponse>(body,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower });

                if (err?.Errors != null && err.Errors.Count > 0)
                {
                    return string.Join(" | ", err.Errors.Select(e => $"[{e.Type}] {e.Message ?? e.Reason}"));
                }
            }
            catch
            {
            }

            switch (statusCode)
            {
                case 400: return "Solicitud inválida a Wompi Payouts. Verifique los datos bancarios.";
                case 401: return "Error de autenticación. Llaves de Payouts inválidas.";
                case 403: return "Acceso denegado. Verifique permisos de la cuenta Wompi.";
                case 404: return "Recurso no encontrado en Wompi Payouts.";
                case 422: return "Datos bancarios inválidos o banco no disponible.";
                case 429: return "Demasiadas solicitudes. Intente en unos segundos.";
                case 500: return "Error interno de Wompi. Intente más tarde.";
                default: return $"Error HTTP {statusCode} de Wompi Payouts. Respuesta: {body}";
            }
        }
    }
}
