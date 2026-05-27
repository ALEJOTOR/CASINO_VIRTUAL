using System;

namespace ENTITY
{
    public class PartidaDisplayDto
    {
        public int IdPartida { get; set; }
        public string Usuario { get; set; }
        public string Juego { get; set; }
        public string Estado { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Apuesta { get; set; }
        public decimal Ganancia { get; set; }
    }
}
