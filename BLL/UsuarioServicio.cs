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
        private readonly RolRepositorio _rolRepositorio = new RolRepositorio();

        // ── Consultas ─────────────────────────────────────────────

        public IList<Usuario> ObtenerTodos()
        {
            return _repositorio.Consultar();
        }

        public Usuario ObtenerPorId(int id)
        {
            foreach (Usuario u in _repositorio.Consultar())
                if (u.IdUsuario == id) return u;
            return null;
        }

        // ── Autenticación ─────────────────────────────────────────

        public Usuario Login(string username, string password)
        {
            string hash = HashPassword(password);
            foreach (Usuario u in _repositorio.Consultar())
                if (u.Username == username && u.Password == hash && u.Estado == "activo")
                    return u;
            return null;
        }

        // ── Alta ──────────────────────────────────────────────────

        public string Registrar(Usuario u)
        {
            if (string.IsNullOrWhiteSpace(u.Username)) return "El username es obligatorio.";
            if (string.IsNullOrWhiteSpace(u.Password)) return "La contraseña es obligatoria.";
            if (string.IsNullOrWhiteSpace(u.Nombre1)) return "El primer nombre es obligatorio.";
            if (string.IsNullOrWhiteSpace(u.Apellido1)) return "El primer apellido es obligatorio.";
            if (string.IsNullOrWhiteSpace(u.Correo)) return "El correo es obligatorio.";
            if (u.IdRol <= 0) return "El rol es obligatorio.";

            foreach (Usuario existente in _repositorio.Consultar())
                if (existente.Username == u.Username)
                    return "El username ya está en uso.";

            u.Password = HashPassword(u.Password);
            u.Estado = "activo";
            u.FechaRegistro = DateTime.Now;
            u.IdUsuario = SiguienteId();

            return _repositorio.Guardar(u);
        }

        // ── Modificación ──────────────────────────────────────────

        public string Actualizar(Usuario uActualizado)
        {
            if (string.IsNullOrWhiteSpace(uActualizado.Nombre1))
                return "El nombre es obligatorio.";

            IList<Usuario> lista = _repositorio.Consultar();
            bool encontrado = false;

            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].IdUsuario == uActualizado.IdUsuario)
                {
                    lista[i] = uActualizado;
                    encontrado = true;
                    break;
                }
            }

            if (!encontrado) return "Usuario no encontrado.";
            return _repositorio.GuardarTodos(lista);
        }

        public string ActualizarSaldo(int idUsuario, decimal delta)
        {
            Usuario u = ObtenerPorId(idUsuario);
            if (u == null) return "Usuario no encontrado.";
            decimal nuevoSaldo = u.Saldo + delta;
            if (nuevoSaldo < 0) return "Saldo insuficiente.";
            u.Saldo = nuevoSaldo;
            return Actualizar(u);
        }

        public string ModificarSaldoAdmin(int idUsuario, decimal nuevoSaldo)
        {
            if (nuevoSaldo < 0) return "El saldo no puede ser negativo.";
            Usuario u = ObtenerPorId(idUsuario);
            if (u == null) return "Usuario no encontrado.";
            u.Saldo = nuevoSaldo;
            return Actualizar(u);
        }

        public string CambiarEstado(int idUsuario, string nuevoEstado)
        {
            Usuario u = ObtenerPorId(idUsuario);
            if (u == null) return "Usuario no encontrado.";
            u.Estado = nuevoEstado;
            return Actualizar(u);
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
            IList<Usuario> lista = _repositorio.Consultar();
            bool encontrado = false;

            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].IdUsuario == idUsuario)
                {
                    lista.RemoveAt(i);
                    encontrado = true;
                    break;
                }
            }

            if (!encontrado) return "Usuario no encontrado.";
            return _repositorio.GuardarTodos(lista);
        }

        // ── Reportes ──────────────────────────────────────────────

        public string GenerarReporte()
        {
            IList<Usuario> lista = _repositorio.Consultar();
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

        // ── Inicialización ────────────────────────────────────────

        public void InicializarAdmin()
        {
            IList<Usuario> lista = _repositorio.Consultar();
            if (lista.Count > 0) return;

            Registrar(new Usuario
            {
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

        private int SiguienteId()
        {
            IList<Usuario> lista = _repositorio.Consultar();
            if (lista.Count == 0) return 1;
            int max = 0;
            foreach (Usuario u in lista)
                if (u.IdUsuario > max) max = u.IdUsuario;
            return max + 1;
        }

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
