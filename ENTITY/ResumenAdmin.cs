using System;

namespace ENTITY
{
    public class ResumenAdmin
    {
        public int TotalUsuarios { get; set; }
        public int UsuariosActivos { get; set; }
        public int UsuariosActivosHoy { get; set; }
        public decimal IngresosHoy { get; set; }
        public decimal IngresosTotal { get; set; }
        public int PartidasHoy { get; set; }
        public int PartidasTotal { get; set; }
        public decimal TotalApostadoHoy { get; set; }
        public decimal GananciaCasaHoy { get; set; }
        public decimal GananciaCasaTotal { get; set; }
        public decimal PromedioApuesta { get; set; }
        public decimal PromedioApuestaHoy { get; set; }
        public string JuegoMasJugado { get; set; }
        public int PartidasJuegoMasJugado { get; set; }
        public string UsuarioMasActivo { get; set; }
        public int PartidasUsuarioMasActivo { get; set; }
    }
}
