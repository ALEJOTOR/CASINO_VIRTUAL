using System;

namespace ENTITY
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Nombre1 { get; set; }
        public string Nombre2 { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string Correo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public decimal Saldo { get; set; }
        public int IdRol { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Estado { get; set; }

        // Navegación
        public Rol Rol { get; set; }
    }
}
