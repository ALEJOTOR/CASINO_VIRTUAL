using System;

namespace ENTITY
{
    public class Juego
    {
        public int IdJuego { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public override string ToString()
        {
            return $"{IdJuego}|{Nombre}|{Descripcion}|{Estado}";
        }
    }
}
