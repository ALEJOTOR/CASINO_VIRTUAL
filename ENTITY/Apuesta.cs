using System;

namespace ENTITY
{
    public class Apuesta
    {
        public int IdApuesta { get; set; }
        public int IdPartida { get; set; }
        public string TipoApuesta { get; set; }
        public int? NumeroApuesta { get; set; }
        public decimal Monto { get; set; }
        public decimal Multiplicador { get; set; }
        public decimal Ganancia { get; set; }
        public string Resultado { get; set; }
    }
}
