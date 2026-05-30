using System;

namespace ENTITY
{
    public class TelegramVinculo
    {
        public int IdVinculo { get; set; }
        public string ChatIdTelegram { get; set; }
        public int IdUsuario { get; set; }
        public string UsernameCasino { get; set; }
        public string Codigo { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public DateTime? FechaConfirmacion { get; set; }
        public DateTime? FechaCancelacion { get; set; }
    }
}
