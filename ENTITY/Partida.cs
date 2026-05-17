using System;

namespace ENTITY
{
    public class Partida
    {
        public int IdPartida { get; set; }
        public int IdUsuario { get; set; }
        public int IdJuego { get; set; }
        public int IdEstado { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Apuesta { get; set; }
        public decimal Ganancia { get; set; }
        public string Resultado { get; set; }

        public override string ToString()
        {
            return string.Join("|",
                IdPartida, IdUsuario, IdJuego, IdEstado,
                Fecha.ToString("yyyy-MM-dd HH:mm:ss"),
                Apuesta.ToString("F2"),
                Ganancia.ToString("F2"),
                Resultado ?? "");
        }
    }
}
