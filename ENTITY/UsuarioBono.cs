using System;

namespace ENTITY
{
    public class UsuarioBono
    {
        public int IdUsuarioBono { get; set; }
        public int IdUsuario { get; set; }
        public string Username { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreBono { get; set; }
        public string TipoBono { get; set; }
        public decimal MontoAplicado { get; set; }
        public DateTime FechaAplicado { get; set; }
        public string Estado { get; set; }
        public string Descripcion { get; set; }
    }
}
