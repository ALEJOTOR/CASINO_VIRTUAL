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

        private static List<BancoWompi> _bancosCacheados;
        private static DateTime _tiempoCache = DateTime.MinValue;
        private static readonly TimeSpan _duracionCache = TimeSpan.FromHours(24);

        public WompiServicio()
            : this(
                new WompiHttpClient(),
                new WompiSignatureService(ConfigurationManager.AppSettings["Wompi.LlaveIntegridad"] ?? ""),
                ConfigurationManager.AppSettings["Wompi.LlavePublica"] ?? "",
                ConfigurationManager.AppSettings["Wompi.LlavePrivada"] ?? "",
                ConfigurationManager.AppSettings["Wompi.Ambiente"] ?? "sandbox"
            )
        {
        }

        public WompiServicio(
            IWompiHttpClient httpClient,
            IWompiSignatureService signatureService,
            string publicKey,
            string privateKey,
            string ambiente)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _signatureService = signatureService ?? throw new ArgumentNullException(nameof(signatureService));
            _publicKey = publicKey ?? "";
            _privateKey = privateKey ?? "";

            bool esProduccion = ambiente?.ToLower() == "produccion" || ambiente?.ToLower() == "production";
            _apiUrl = esProduccion ? "https://api.wompi.co/v1" : "https://sandbox.wompi.co/v1";
        }

        public bool TieneLlavesValidas()
        {
            return !string.IsNullOrEmpty(_publicKey)
                && !string.IsNullOrEmpty(_privateKey);
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

        public Task<(bool ok, string payoutId, string error)> CrearPagoTerceroAsync(decimal monto, DatosBancarios datos, string referencia)
        {
            string idInterno = $"RETIRO-MANUAL-{referencia}";
            return Task.FromResult((true, idInterno, (string)null));
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

        public async Task<(bool ok, string estado, string error)> ConsultarEstadoPagoTerceroAsync(string payoutId)
        {
            return await Task.FromResult((true, "PENDING", (string)null));
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
    }
}
