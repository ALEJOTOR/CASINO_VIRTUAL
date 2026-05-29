using System;

namespace ENTITY
{
    public class DatosBancarios
    {
        public int IdDatosBancarios { get; set; }
        public int IdUsuario { get; set; }
        public string BancoId { get; set; }
        public string BancoNombre { get; set; }
        public string TipoCuenta { get; set; }
        public string NumeroCuenta { get; set; }
        public string TipoDoc { get; set; }
        public string NumeroDoc { get; set; }
        public string NombreTitular { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
