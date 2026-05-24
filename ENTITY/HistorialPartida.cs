using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class HistorialPartida
    {
        public int IdPartida { get; set; }
        public int IdUsuario { get; set; }
        public string NombreJuego { get; set; }
        public string Estado { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Apuesta { get; set; }
        public decimal Ganancia { get; set; }
        public string Resultado { get; set; }
    }
}
