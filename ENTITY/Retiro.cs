using System;

namespace ENTITY
{
    public class Retiro
    {
        public int IdRetiro { get; set; }
        public int IdUsuario { get; set; }
        public string Username { get; set; }
        public string NombreUsuario { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaProcesamiento { get; set; }
        public string MetodoPago { get; set; }
        public string AdminRevisor { get; set; }
    }
}
