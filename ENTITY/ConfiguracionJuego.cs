using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{

    public class ConfiguracionJuego
    {
        public int IdConfig { get; set; }
        public int IdJuego { get; set; }
        public string NombreParametro { get; set; }
        public string Valor { get; set; }
    }
}
