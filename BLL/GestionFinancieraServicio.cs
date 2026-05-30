using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class GestionFinancieraServicio
    {
        private readonly DepositoRepositorio _depositoRepo = new DepositoRepositorio();
        private readonly RetiroRepositorio _retiroRepo = new RetiroRepositorio();
        private readonly DatosBancariosRepositorio _datosBancariosRepo = new DatosBancariosRepositorio();
        private readonly MetodoPagoRepositorio _metodoPagoRepo = new MetodoPagoRepositorio();
        private readonly WompiServicio _wompiSvc = new WompiServicio();
        private readonly UsuarioServicio _usuarioSvc = new UsuarioServicio();
        private readonly LogServicio _logSvc = new LogServicio();

        private const decimal MontoMaximoDeposito = 5000000m;

        public (string mensaje, int idDeposito) SolicitarDeposito(int idUsuario, decimal monto, int? idMetodo, string descripcion)
        {
            if (monto <= 0)
                return ("El monto debe ser mayor a 0.", 0);
            if (monto > MontoMaximoDeposito)
                return ($"El monto maximo por deposito es ${MontoMaximoDeposito:N2}.", 0);

            Usuario u = _usuarioSvc.ObtenerPorId(idUsuario);
            if (u == null)
                return ("Usuario no encontrado.", 0);
            if (u.Estado != "activo")
                return ("El usuario no esta activo.", 0);

            var (resultado, idDeposito) = _depositoRepo.Solicitar(idUsuario, monto, idMetodo, descripcion);

            if (resultado == "Guardado correctamente.")
            {
                _logSvc.Registrar(LogEventos.Deposito, LogEventos.Info, "BILLETERA",
                    $"Solicitud de deposito ${monto} creada (ID: {idDeposito})", idUsuario);
            }

            return (resultado, idDeposito);
        }

        public string ConfirmarDeposito(int idDeposito, string refWompi)
        {
            return _depositoRepo.Confirmar(idDeposito, refWompi);
        }

        public string RechazarDeposito(int idDeposito, string motivo)
        {
            return _depositoRepo.Rechazar(idDeposito, motivo);
        }

        public (string mensaje, int idRetiro) SolicitarRetiro(int idUsuario, decimal monto, int? idMetodo)
        {
            if (monto <= 0)
                return ("El monto debe ser mayor a 0.", 0);

            Usuario u = _usuarioSvc.ObtenerPorId(idUsuario);
            if (u == null)
                return ("Usuario no encontrado.", 0);
            if (u.Saldo < monto)
                return ("Saldo insuficiente.", 0);

            var (resultado, idRetiro) = _retiroRepo.Solicitar(idUsuario, monto, idMetodo);

            if (resultado == "Guardado correctamente.")
            {
                _logSvc.Registrar(LogEventos.Retiro, LogEventos.Info, "BILLETERA",
                    $"Solicitud de retiro ${monto} creada (ID: {idRetiro})", idUsuario);
            }

            return (resultado, idRetiro);
        }

        public string AprobarRetiro(int idRetiro, int? idAdmin, string refWompi)
        {
            return _retiroRepo.Aprobar(idRetiro, idAdmin, refWompi);
        }

        public string RechazarRetiro(int idRetiro, string motivo)
        {
            return _retiroRepo.Rechazar(idRetiro, motivo);
        }

        /// <summary>
        /// Solicita un retiro con procesamiento AUTOMÁTICO a través de Wompi Payouts.
        /// Flujo:
        /// 1. Valida monto y usuario
        /// 2. Verifica saldo disponible
        /// 3. Obtiene datos bancarios del usuario
        /// 4. Llama a Wompi Payouts API (si Wompi acepta, saldo se descuenta aquí)
        /// 5. Registra el retiro en BD y lo aprueba automáticamente
        /// 6. No requiere intervención de administrador
        /// </summary>
        public async Task<(bool ok, string mensaje, int idRetiro)> SolicitarRetiroConWompiAsync(
            int idUsuario, decimal monto, int? idMetodo)
        {
            if (monto <= 0)
                return (false, "El monto debe ser mayor a 0.", 0);

            // Obtener usuario
            Usuario u = _usuarioSvc.ObtenerPorId(idUsuario);
            if (u == null)
                return (false, "Usuario no encontrado.", 0);

            // Validación previa: verifica saldo antes de tocar Wompi
            if (u.Saldo < monto)
                return (false, "Saldo insuficiente para procesar este retiro.", 0);

            // Obtener datos bancarios registrados
            DatosBancarios datosBancarios = _datosBancariosRepo.ObtenerActivoPorUsuario(idUsuario);
            if (datosBancarios == null)
                return (false, "Debes registrar un método de pago antes de retirar.", 0);

            // Generar referencia única
            string referencia = $"RETIRO-{idUsuario}-{DateTime.Now:yyyyMMddHHmmss}";

            try
            {
                // Llamar a Wompi Payouts ANTES de tocar la BD
                // Si Wompi rechaza, aquí se retorna el error y la BD no se modifica
                var (okWompi, payoutId, errorWompi) = await _wompiSvc.CrearPagoTerceroAsync(
                    monto, datosBancarios, referencia, u.Correo);

                if (!okWompi)
                {
                    _logSvc.Registrar(LogEventos.Retiro, LogEventos.Error, "WOMPI",
                        $"Payout rechazado para usuario {idUsuario}: {errorWompi}", idUsuario);
                    return (false, $"Wompi rechazó el pago: {errorWompi}", 0);
                }

                // Wompi aceptó. Ahora registrar en BD:
                // 1. Insertar retiro (ejecuta PR_SOLICITAR_RETIRO que inserta en transacciones + descuenta saldo vía trigger)
                var (resultadoSolicitud, idRetiro) = _retiroRepo.Solicitar(idUsuario, monto, idMetodo);

                if (resultadoSolicitud != "Guardado correctamente.")
                {
                    _logSvc.Registrar(LogEventos.Retiro, LogEventos.Error, "BD",
                        $"Error insertar retiro en BD: {resultadoSolicitud}. Payout ya fue enviado: {payoutId}", idUsuario);
                    // Nota: en producción, habría que considerar una tabla de errores para reconciliación
                    return (false, 
                        $"Error interno al registrar retiro (pago ya fue enviado a Wompi). Contacte soporte con ID: {payoutId}", 
                        0);
                }

                // 2. Aprobar inmediatamente (idAdmin = null indica aprobación automática)
                string resultadoAprobacion = _retiroRepo.Aprobar(idRetiro, null, payoutId);

                if (!resultadoAprobacion.Contains("correctamente"))
                {
                    _logSvc.Registrar(LogEventos.Retiro, LogEventos.Error, "BD",
                        $"Error aprobar retiro en BD: {resultadoAprobacion}. Payout: {payoutId}", idUsuario);
                    return (false, $"Error al procesar retiro: {resultadoAprobacion}", 0);
                }

                _logSvc.Registrar(LogEventos.Retiro, LogEventos.Info, "WOMPI",
                    $"Retiro ${monto:N2} procesado exitosamente. Payout Wompi: {payoutId}", idUsuario);

                return (true, 
                    $"Retiro procesado exitosamente. Tu dinero será transferido a tu cuenta bancaria. " +
                    $"ID de pago: {payoutId}", 
                    idRetiro);
            }
            catch (Exception ex)
            {
                _logSvc.Registrar(LogEventos.Retiro, LogEventos.Error, "EXCEPTION",
                    $"Excepción en SolicitarRetiroConWompiAsync: {ex.Message}", idUsuario);
                return (false, $"Error inesperado: {ex.Message}", 0);
            }
        }

        public IList<Deposito> ObtenerDepositosPendientes()
        {
            return _depositoRepo.ObtenerPendientes();
        }

        public IList<Retiro> ObtenerRetirosPendientes()
        {
            return _retiroRepo.ObtenerPendientes();
        }

        public IList<Deposito> ObtenerDepositosPorUsuario(int idUsuario)
        {
            return _depositoRepo.ObtenerPorUsuario(idUsuario);
        }

        public IList<Retiro> ObtenerRetirosPorUsuario(int idUsuario)
        {
            return _retiroRepo.ObtenerPorUsuario(idUsuario);
        }

        public IList<MetodoPago> ObtenerMetodosPago()
        {
            return _metodoPagoRepo.Consultar();
        }

        public string GuardarDatosBancarios(DatosBancarios datos)
        {
            return _datosBancariosRepo.Guardar(datos);
        }

        public IList<DatosBancarios> ObtenerDatosBancarios(int idUsuario)
        {
            return _datosBancariosRepo.ObtenerPorUsuario(idUsuario);
        }

        public DatosBancarios ObtenerDatosBancariosActivos(int idUsuario)
        {
            return _datosBancariosRepo.ObtenerActivoPorUsuario(idUsuario);
        }

        public void ActualizarReferenciaWompiDeposito(int idDeposito, string idLink)
        {
            _depositoRepo.ActualizarReferenciaWompi(idDeposito, idLink);
        }

        public async Task<(bool ok, string link, string idLink, string error)> CrearLinkPagoWompiAsync(decimal monto, string referencia)
        {
            return await _wompiSvc.CrearLinkPagoAsync(monto, referencia);
        }

        public async Task<(bool ok, string estado, string error)> VerificarTransaccionWompiAsync(string idLink)
        {
            var (ok, transacciones, error) = await _wompiSvc.ConsultarTransaccionesLinkAsync(idLink);
            if (!ok || transacciones == null || transacciones.Count == 0)
                return (false, null, error ?? "Sin transacciones asociadas.");

            var aprobada = transacciones.Find(t => t.Status == "APPROVED");
            if (aprobada != null)
                return (true, "APPROVED", null);

            var fallida = transacciones.Find(t => t.Status == "DECLINED" || t.Status == "ERROR");
            if (fallida != null)
                return (true, fallida.Status, null);

            return (true, "PENDING", null);
        }

        public async Task<(bool ok, string mensaje)> AprobarRetiroConWompiAsync(int idRetiro, int idAdmin)
        {
            try
            {
                var retiros = _retiroRepo.ObtenerPendientes();
                Retiro retiro = null;
                foreach (var r in retiros)
                {
                    if (r.IdRetiro == idRetiro)
                    {
                        retiro = r;
                        break;
                    }
                }

                if (retiro == null)
                    return (false, "Retiro no encontrado o ya procesado.");

                string referencia = $"RETIRO-{idRetiro}-{DateTime.Now:yyyyMMddHHmmss}";
                Usuario usuario = _usuarioSvc.ObtenerPorId(retiro.IdUsuario);
                if (usuario == null)
                    return (false, "Usuario no encontrado.");

                DatosBancarios datos = _datosBancariosRepo.ObtenerActivoPorUsuario(retiro.IdUsuario);
                if (datos == null)
                    return (false, "El usuario no tiene datos bancarios registrados.");

                var (ok, payoutId, error) = await _wompiSvc.CrearPagoTerceroAsync(retiro.Monto, datos, referencia);
                if (!ok)
                {
                    _logSvc.Registrar(LogEventos.Retiro, LogEventos.Error, "WOMPI",
                        $"Pago a tercero fallido para retiro {idRetiro}: {error}", idAdmin);
                    return (false, $"Wompi rechazo el pago: {error}");
                }

                string resultado = _retiroRepo.Aprobar(idRetiro, idAdmin, payoutId);
                bool exito = resultado.Contains("correctamente");

                _logSvc.Registrar(LogEventos.Retiro, exito ? LogEventos.Info : LogEventos.Error, "WOMPI",
                    exito
                        ? $"Retiro {idRetiro} aprobado con Payout Wompi: {payoutId}"
                        : $"Retiro {idRetiro} aprobado en BD pero error: {resultado}",
                    idAdmin);

                return (exito, exito ? $"Pago enviado por Wompi. ID: {payoutId}" : resultado);
            }
            catch (Exception ex)
            {
                _logSvc.Registrar(LogEventos.Retiro, LogEventos.Error, "WOMPI",
                    $"Excepcion en AprobarRetiroConWompiAsync: {ex.Message}", idAdmin);
                return (false, $"Error interno: {ex.Message}");
            }
        }
    }
}
