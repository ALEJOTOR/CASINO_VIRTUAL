using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class DetallePartida
    {
        public int IdDetalle { get; set; }
        public int IdPartida { get; set; }
        public string NombreParametro { get; set; }
        public string Valor { get; set; }
    }
}
