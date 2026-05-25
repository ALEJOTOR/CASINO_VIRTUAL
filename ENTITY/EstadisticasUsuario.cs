namespace ENTITY
{
    public class EstadisticasUsuario
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Username { get; set; }
        public decimal Saldo { get; set; }
        public int TotalPartidas { get; set; }
        public int PartidasGanadas { get; set; }
        public int PartidasPerdidas { get; set; }
        public decimal TotalApostado { get; set; }
        public decimal TotalGanado { get; set; }
        public decimal GananciaNeta { get; set; }
        public string JuegoFavorito { get; set; }
        public int RachaActual { get; set; }
        public string TipoRacha { get; set; }
    }
}
