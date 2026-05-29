using System;

namespace ENTITY
{
    public class Deposito
    {
        public int IdDeposito { get; set; }
        public int IdUsuario { get; set; }
        public string Username { get; set; }
        public string NombreUsuario { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaProcesamiento { get; set; }
        public string MetodoPago { get; set; }
        public string Descripcion { get; set; }
    }
}
