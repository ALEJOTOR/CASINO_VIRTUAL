using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class UsuarioBLL
    {
        private readonly UsuarioDAL _dal = new UsuarioDAL();

        public (bool ok, string msg) Registrar(Usuario u)
        {
            if (string.IsNullOrWhiteSpace(u.Username))  return (false, "El username es obligatorio.");
            if (string.IsNullOrWhiteSpace(u.Password))  return (false, "La contraseña es obligatoria.");
            if (string.IsNullOrWhiteSpace(u.Nombre1))   return (false, "El primer nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(u.Apellido1)) return (false, "El primer apellido es obligatorio.");
            if (string.IsNullOrWhiteSpace(u.Correo))    return (false, "El correo es obligatorio.");
            if (u.IdRol <= 0)                            return (false, "El rol es obligatorio.");

            foreach (var existente in _dal.ConsultarTodos())
                if (existente.Username == u.Username)
                    return (false, "El username ya está en uso.");

            u.Password = HashPassword(u.Password);
            u.Estado   = "activo";
            bool ok    = _dal.Insertar(u);
            return ok ? (true, "Usuario registrado correctamente.") : (false, "Error al registrar.");
        }

        public Usuario Login(string username, string password)
        {
            return _dal.Login(username, HashPassword(password));
        }

        public List<Usuario> ObtenerTodos() => _dal.ConsultarTodos();
        public Usuario ObtenerPorId(int id) => _dal.ConsultarPorId(id);

        public (bool ok, string msg) Actualizar(Usuario u)
        {
            if (string.IsNullOrWhiteSpace(u.Nombre1)) return (false, "El nombre es obligatorio.");
            bool ok = _dal.Actualizar(u);
            return ok ? (true, "Actualizado.") : (false, "Error al actualizar.");
        }

        public (bool ok, string msg) Eliminar(int id)
        {
            bool ok = _dal.Eliminar(id);
            return ok ? (true, "Eliminado.") : (false, "Error al eliminar.");
        }

        public (bool ok, string msg) ActualizarSaldo(int idUsuario, decimal monto)
        {
            var u = _dal.ConsultarPorId(idUsuario);
            if (u == null) return (false, "Usuario no encontrado.");
            decimal nuevoSaldo = u.Saldo + monto;
            if (nuevoSaldo < 0) return (false, "Saldo insuficiente.");
            bool ok = _dal.ActualizarSaldo(idUsuario, nuevoSaldo);
            return ok ? (true, "Saldo actualizado.") : (false, "Error al actualizar saldo.");
        }

        private string HashPassword(string password)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        // ── Métodos nuevos admin ──────────────────────────────────
        public (bool ok, string msg) CambiarEstado(int idUsuario, string nuevoEstado)
        {
            var u = _dal.ConsultarPorId(idUsuario);
            if (u == null) return (false, "Usuario no encontrado.");
            u.Estado = nuevoEstado;
            bool ok = _dal.Actualizar(u);
            return ok ? (true, $"Estado cambiado a '{nuevoEstado}'.") : (false, "Error al cambiar estado.");
        }

        public (bool ok, string msg) ModificarSaldoAdmin(int idUsuario, decimal nuevoSaldo)
        {
            if (nuevoSaldo < 0) return (false, "El saldo no puede ser negativo.");
            var u = _dal.ConsultarPorId(idUsuario);
            if (u == null) return (false, "Usuario no encontrado.");
            bool ok = _dal.ActualizarSaldo(idUsuario, nuevoSaldo);
            return ok ? (true, "Saldo modificado correctamente.") : (false, "Error al modificar saldo.");
        }

        public (bool ok, string msg) CambiarPassword(int idUsuario, string nuevaPassword)
        {
            if (string.IsNullOrWhiteSpace(nuevaPassword)) return (false, "La contraseña no puede estar vacía.");
            var u = _dal.ConsultarPorId(idUsuario);
            if (u == null) return (false, "Usuario no encontrado.");
            u.Password = HashPassword(nuevaPassword);
            bool ok = _dal.Actualizar(u);
            return ok ? (true, "Contraseña cambiada.") : (false, "Error al cambiar contraseña.");
        }

        public string GenerarReporteUsuarios()
        {
            var lista = _dal.ConsultarTodos();
            int activos = lista.Count(u => u.Estado == "activo");
            int inactivos = lista.Count(u => u.Estado == "inactivo");
            int suspendidos = lista.Count(u => u.Estado == "suspendido");
            decimal totalSaldos = lista.Sum(u => u.Saldo);
            return $"REPORTE DE USUARIOS\n" +
                   $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}\n" +
                   $"─────────────────────────────\n" +
                   $"Total usuarios:    {lista.Count}\n" +
                   $"Activos:           {activos}\n" +
                   $"Inactivos:         {inactivos}\n" +
                   $"Suspendidos:       {suspendidos}\n" +
                   $"Total saldos:      ${totalSaldos:N2}\n";
        }
    }

    // ─────────────────────────────────────────────────────────────
    public class PartidaBLL
    {
        private readonly PartidaDAL     _partidaDal = new PartidaDAL();
        private readonly TransaccionDAL _transDal   = new TransaccionDAL();
        private readonly UsuarioBLL     _usuarioBll = new UsuarioBLL();

        public (bool ok, string msg) RegistrarPartida(Partida p)
        {
            if (p.Apuesta <= 0) return (false, "La apuesta debe ser mayor a 0.");
            var usuario = _usuarioBll.ObtenerPorId(p.IdUsuario);
            if (usuario == null)           return (false, "Usuario no encontrado.");
            if (usuario.Saldo < p.Apuesta) return (false, "Saldo insuficiente.");

            _usuarioBll.ActualizarSaldo(p.IdUsuario, -p.Apuesta);
            _transDal.Insertar(new Transaccion
            {
                IdUsuario   = p.IdUsuario,
                Tipo        = "pérdida",
                Monto       = p.Apuesta,
                Descripcion = $"Apuesta partida juego {p.IdJuego}"
            });

            if (p.Resultado == "ganó" && p.Ganancia > 0)
            {
                _usuarioBll.ActualizarSaldo(p.IdUsuario, p.Ganancia);
                _transDal.Insertar(new Transaccion
                {
                    IdUsuario   = p.IdUsuario,
                    Tipo        = "ganancia",
                    Monto       = p.Ganancia,
                    Descripcion = $"Ganancia partida juego {p.IdJuego}"
                });
            }

            bool ok = _partidaDal.Insertar(p);
            return ok ? (true, "Partida registrada.") : (false, "Error al guardar.");
        }

        public (bool ok, string msg) RealizarDeposito(int idUsuario, decimal monto)
        {
            if (monto <= 0) return (false, "El monto debe ser mayor a 0.");
            var resultado = _usuarioBll.ActualizarSaldo(idUsuario, monto);
            if (!resultado.ok) return resultado;

            _transDal.Insertar(new Transaccion
            {
                IdUsuario   = idUsuario,
                Tipo        = "depósito",
                Monto       = monto,
                Descripcion = "Recarga de saldo"
            });
            return (true, "Depósito realizado.");
        }

        public List<Partida> ObtenerPartidasUsuario(int id) => _partidaDal.ConsultarPorUsuario(id);
        public List<Partida> ObtenerTodasPartidas()         => _partidaDal.ConsultarTodos();
        public List<Transaccion> ObtenerTransacciones(int id) => _transDal.ConsultarPorUsuario(id);

        public List<Transaccion> ObtenerTodasTransacciones()
        {
            var todas = new List<Transaccion>();
            var usuarios = new UsuarioBLL().ObtenerTodos();
            foreach (var u in usuarios)
                todas.AddRange(_transDal.ConsultarPorUsuario(u.IdUsuario));
            return todas;
        }

        public string GenerarReportePartidas()
        {
            var partidas = ObtenerTodasPartidas();
            int ganadas = partidas.Count(p => p.Resultado == "ganó");
            int perdidas = partidas.Count(p => p.Resultado == "perdió");
            decimal totalApuestas = partidas.Sum(p => p.Apuesta);
            decimal totalGanancias = partidas.Sum(p => p.Ganancia);
            return $"REPORTE DE PARTIDAS\n" +
                   $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}\n" +
                   $"─────────────────────────────\n" +
                   $"Total partidas:    {partidas.Count}\n" +
                   $"Ganadas:           {ganadas}\n" +
                   $"Perdidas:          {perdidas}\n" +
                   $"Total apostado:    ${totalApuestas:N2}\n" +
                   $"Total ganado:      ${totalGanancias:N2}\n" +
                   $"Balance casino:    ${totalApuestas - totalGanancias:N2}\n";
        }
    }

    // ─────────────────────────────────────────────────────────────
    /// <summary>Crea los archivos de datos iniciales si no existen.</summary>
    public class InicializadorBLL
    {
        public void InicializarTodo()
        {
            new RolDAL().InicializarDatos();
            new JuegoDAL().InicializarDatos();
            new EstadoPartidaDAL().InicializarDatos();
            new UsuarioDAL().InicializarAdmin();
        }
    }
}
