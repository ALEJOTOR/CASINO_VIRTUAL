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
                _logSvc.Registrar("deposito", "INFO", "BILLETERA",
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
                _logSvc.Registrar("retiro", "INFO", "BILLETERA",
                    $"Solicitud de retiro ${monto} creada (ID: {idRetiro})", idUsuario);
            }

            return (resultado, idRetiro);
        }

        public string AprobarRetiro(int idRetiro, int idAdmin, string refWompi)
        {
            return _retiroRepo.Aprobar(idRetiro, idAdmin, refWompi);
        }

        public string RechazarRetiro(int idRetiro, string motivo)
        {
            return _retiroRepo.Rechazar(idRetiro, motivo);
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
                    _logSvc.Registrar("retiro", "ERROR", "WOMPI",
                        $"Pago a tercero fallido para retiro {idRetiro}: {error}", idAdmin);
                    return (false, $"Wompi rechazo el pago: {error}");
                }

                string resultado = _retiroRepo.Aprobar(idRetiro, idAdmin, payoutId);
                bool exito = resultado.Contains("correctamente");

                _logSvc.Registrar("retiro", exito ? "INFO" : "ERROR", "WOMPI",
                    exito
                        ? $"Retiro {idRetiro} aprobado con Payout Wompi: {payoutId}"
                        : $"Retiro {idRetiro} aprobado en BD pero error: {resultado}",
                    idAdmin);

                return (exito, exito ? $"Pago enviado por Wompi. ID: {payoutId}" : resultado);
            }
            catch (Exception ex)
            {
                _logSvc.Registrar("retiro", "ERROR", "WOMPI",
                    $"Excepcion en AprobarRetiroConWompiAsync: {ex.Message}", idAdmin);
                return (false, $"Error interno: {ex.Message}");
            }
        }
    }
}
