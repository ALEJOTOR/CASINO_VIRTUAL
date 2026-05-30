using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BonoServicio
    {
        private readonly BonoRepositorio _bonoRepo = new BonoRepositorio();
        private readonly UsuarioServicio _usuarioSvc = new UsuarioServicio();
        private readonly LogServicio _logSvc = new LogServicio();

        public IList<Bono> ObtenerBonosActivos()
        {
            return _bonoRepo.ObtenerActivos();
        }

        public IList<Bono> ObtenerTodos()
        {
            return _bonoRepo.Consultar();
        }

        public string CrearBono(Bono bono)
        {
            if (string.IsNullOrWhiteSpace(bono.Nombre))
                return "El nombre del bono es obligatorio.";
            if (bono.Valor <= 0)
                return "El valor del bono debe ser mayor a 0.";

            return _bonoRepo.Guardar(bono);
        }

        public string ActualizarBono(Bono bono)
        {
            if (bono.IdBono <= 0)
                return "Bono no válido.";

            return _bonoRepo.Actualizar(bono);
        }

        public string DesactivarBono(int idBono)
        {
            return _bonoRepo.Desactivar(idBono);
        }

        public string RevocarBono(int idUsuarioBono, string motivo)
        {
            return _bonoRepo.RevocarBono(idUsuarioBono, motivo);
        }

        public string AplicarBonoManual(int idUsuario, int idBono, decimal monto, string descripcion)
        {
            if (monto <= 0)
                return "El monto debe ser mayor a 0.";

            Usuario usuario = _usuarioSvc.ObtenerPorId(idUsuario);
            if (usuario == null)
                return "Usuario no encontrado.";

            Bono bono = _bonoRepo.Consultar().FirstOrDefault(b => b.IdBono == idBono);
            if (bono == null || !bono.Activo)
                return "El bono no existe o no esta activo.";

            return _bonoRepo.AplicarBono(idUsuario, idBono, monto, descripcion);
        }

        public string AplicarBonoConFecha(int idUsuario, int idBono, decimal monto, DateTime? fechaExpiracion, string descripcion)
        {
            string resultado = AplicarBonoManual(idUsuario, idBono, monto, descripcion);
            if (resultado.Contains("correctamente") && fechaExpiracion.HasValue)
            {
                _bonoRepo.ActualizarFechaExpiracion(idUsuario, idBono, fechaExpiracion.Value);
            }
            return resultado;
        }

        public string AplicarBonoBienvenida(int idUsuario)
        {
            Bono bonoBienvenida = _bonoRepo.ObtenerPorTipo("bienvenida");
            if (bonoBienvenida == null)
                return "No hay bono de bienvenida configurado.";

            return _bonoRepo.AplicarBono(idUsuario, bonoBienvenida.IdBono, bonoBienvenida.Valor,
                "Bono de bienvenida al registrarse");
        }

        public IList<UsuarioBono> ObtenerHistorialBonos(int idUsuario)
        {
            return _bonoRepo.ObtenerBonosPorUsuario(idUsuario);
        }

        public IList<UsuarioBono> ObtenerTodosParaAdmin()
        {
            return _bonoRepo.ObtenerTodosLosBonosAplicados();
        }

        public (int total, int activos, decimal montoTotal, int usuarios) ObtenerResumen()
        {
            return _bonoRepo.ObtenerResumen();
        }
    }
}
