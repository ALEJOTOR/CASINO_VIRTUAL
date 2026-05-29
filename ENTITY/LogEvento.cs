using System;

namespace ENTITY
{
    public class LogEvento
    {
        public int IdLog { get; set; }
        public string TipoEvento { get; set; }
        public string Nivel { get; set; }
        public int? IdUsuario { get; set; }
        public string Username { get; set; }
        public string IpOrigen { get; set; }
        public string Modulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
    }
}
