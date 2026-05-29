using System;

namespace ENTITY
{
    public class ApuestaDisplayDto
    {
        public int IdApuesta { get; set; }
        public int IdPartida { get; set; }
        public string Username { get; set; }
        public string NombreUsuario { get; set; }
        public string Juego { get; set; }
        public string TipoApuesta { get; set; }
        public int? NumeroApuesta { get; set; }
        public decimal Monto { get; set; }
        public decimal Multiplicador { get; set; }
        public decimal Ganancia { get; set; }
        public string Resultado { get; set; }
        public DateTime Fecha { get; set; }
    }
}
