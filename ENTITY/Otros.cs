using System;

namespace ENTITY
{
    public class Juego
    {
        public int IdJuego { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
    }

    public class EstadoPartida
    {
        public int IdEstado { get; set; }
        public string NombreEstado { get; set; }
        public string Descripcion { get; set; }
    }

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

        // Navegación
        public Usuario Usuario { get; set; }
        public Juego Juego { get; set; }
        public EstadoPartida EstadoPartida { get; set; }
    }

    public class Transaccion
    {
        public int IdTransaccion { get; set; }
        public int IdUsuario { get; set; }
        public string Tipo { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }

        // Navegación
        public Usuario Usuario { get; set; }
    }
}
