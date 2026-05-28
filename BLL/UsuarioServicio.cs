using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BLL
{
    public class UsuarioServicio
    {
        private readonly UsuarioRepositorio _repositorio = new UsuarioRepositorio();
        private readonly EstadoUsuarioRepositorio _estadoRepo = new EstadoUsuarioRepositorio();
        private readonly RolRepositorio _rolRepositorio = new RolRepositorio();
        private readonly TransaccionRepositorio _transaccionRepositorio = new TransaccionRepositorio();

        public IList<EstadoUsuario> ObtenerEstados()
        {
            return _estadoRepo.Consultar();
        }

        // ── Consultas ─────────────────────────────────────────────

        public IList<Usuario> ObtenerTodos()
        {
            return _repositorio.Consultar();
        }

        public IList<Usuario> ObtenerActivos()
        {
            return _repositorio.Consultar().Where(u => u.Estado == "activo").ToList();
        }

        public Usuario ObtenerPorId(int id)
        {
            return _repositorio.ObtenerPorId(id);
        }

        // ── Autenticación ─────────────────────────────────────────

        public Usuario Login(string username, string password)
        {
            string hash = HashPassword(password);
            Usuario u = _repositorio.ObtenerPorUsername(username);

            if (u != null && u.Password == hash && u.Estado == "activo")
                return u;

            return null;
        }

        // ── Alta ──────────────────────────────────────────────────

        public string Registrar(Usuario u)
        {
            if (u.IdUsuario < 1) return "La identificación es obligatoria y no puede ser un número negativo";
            if (string.IsNullOrWhiteSpace(u.Username)) return "El username es obligatorio.";
            if (string.IsNullOrWhiteSpace(u.Password)) return "La contraseña es obligatoria.";
            if (string.IsNullOrWhiteSpace(u.Nombre1)) return "El primer nombre es obligatorio.";
            if (string.IsNullOrWhiteSpace(u.Apellido1)) return "El primer apellido es obligatorio.";
            if (string.IsNullOrWhiteSpace(u.Correo)) return "El correo es obligatorio.";
            if (u.IdRol <= 0) return "El rol es obligatorio.";

            if (_repositorio.ObtenerPorUsername(u.Username) != null)
                return "El username ya está en uso.";

            if(_repositorio.ObtenerPorId(u.IdUsuario) != null){
                return "La identificación ya está registrada.";
            }

            u.Password = HashPassword(u.Password);
            u.Estado = "activo";
            u.FechaRegistro = DateTime.Now;
            u.Saldo = 0;

            return _repositorio.Guardar(u);
        }

        // ── Modificación ──────────────────────────────────────────

        public string Actualizar(Usuario uActualizado)
        {
            if (string.IsNullOrWhiteSpace(uActualizado.Nombre1))
                return "El nombre es obligatorio.";

            Usuario existente = ObtenerPorId(uActualizado.IdUsuario);
            if (existente == null) return "Usuario no encontrado.";

            if (string.IsNullOrWhiteSpace(uActualizado.Password))
                uActualizado.Password = existente.Password;

            return _repositorio.Actualizar(uActualizado);
        }

        public string ActualizarSaldo(int idUsuario, decimal delta)
        {
            Usuario u = ObtenerPorId(idUsuario);
            if (u == null) return "Usuario no encontrado.";

            if (delta == 0) return "Guardado correctamente.";
            if (u.Saldo + delta < 0) return "Saldo insuficiente.";

            return _transaccionRepositorio.Guardar(new Transaccion
            {
                IdUsuario = idUsuario,
                Tipo = delta > 0 ? "deposito" : "retiro",
                Monto = Math.Abs(delta),
                Fecha = DateTime.Now,
                Descripcion = delta > 0
                    ? "Ajuste positivo de saldo por administrador"
                    : "Ajuste negativo de saldo por administrador"
            });
        }

        public string RetirarSaldo(int idUsuario, decimal monto)
        {
            if (monto <= 0) return "El monto debe ser mayor a 0.";

            Usuario u = ObtenerPorId(idUsuario);
            if (u == null) return "Usuario no encontrado.";
            if (u.Saldo < monto) return "Saldo insuficiente.";

            return _transaccionRepositorio.Guardar(new Transaccion
            {
                IdUsuario = idUsuario,
                Tipo = "retiro",
                Monto = monto,
                Fecha = DateTime.Now,
                Descripcion = "Retiro de saldo"
            });
        }

        public string ModificarSaldoAdmin(int idUsuario, decimal nuevoSaldo)
        {
            if (nuevoSaldo < 0) return "El saldo no puede ser negativo.";
            Usuario u = ObtenerPorId(idUsuario);
            if (u == null) return "Usuario no encontrado.";

            decimal diferencia = nuevoSaldo - u.Saldo;
            return ActualizarSaldo(idUsuario, diferencia);
        }

        public string CambiarEstado(int idUsuario, string nuevoEstado)
        {
            return _repositorio.CambiarEstado(idUsuario, nuevoEstado);
        }

        public string CambiarPassword(int idUsuario, string nuevaPassword)
        {
            if (string.IsNullOrWhiteSpace(nuevaPassword))
                return "La contraseña no puede estar vacía.";
            Usuario u = ObtenerPorId(idUsuario);
            if (u == null) return "Usuario no encontrado.";
            u.Password = HashPassword(nuevaPassword);
            return Actualizar(u);
        }

        // ── Baja ──────────────────────────────────────────────────

        public string Eliminar(int idUsuario)
        {
            Usuario existente = ObtenerPorId(idUsuario);
            if (existente == null) return "Usuario no encontrado.";

            return _repositorio.Eliminar(idUsuario);
        }

        // ── Inicialización ────────────────────────────────────────

        public void InicializarAdmin()
        {
            IList<Usuario> lista = _repositorio.Consultar();
            if (lista.Count > 0) return;

            Registrar(new Usuario
            {
                IdUsuario = 1,
                Username = "admin",
                Password = "admin123",   // Registrar() hará el hash
                Nombre1 = "Admin",
                Apellido1 = "Casino",
                Correo = "admin@casino.com",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Saldo = 0,
                IdRol = 1
            });
        }

        // ── Helpers privados ──────────────────────────────────────

        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}
