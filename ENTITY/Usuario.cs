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

        public bool EsMayorDeEdad()
        {
            int edad = DateTime.Today.Year - FechaNacimiento.Year;

            // Si aún no ha cumplido años este año, se resta 1
            if (FechaNacimiento.Date > DateTime.Today.AddYears(-edad))
            {
                edad--;
            }

            return edad >= 18;
        }
        public override string ToString()
        {
            return string.Join("|",
                IdUsuario,
                Username,
                Password,
                Nombre1,
                Nombre2 ?? "",
                Apellido1,
                Apellido2 ?? "",
                Correo,
                FechaNacimiento.ToString("yyyy-MM-dd"),
                Saldo.ToString("F2"),
                IdRol,
                FechaRegistro.ToString("yyyy-MM-dd"),
                Estado);
        }
    }
}
