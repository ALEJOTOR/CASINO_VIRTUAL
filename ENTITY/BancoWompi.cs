using System.Collections.Generic;

namespace ENTITY
{
    public class BancoWompi
    {
        public string Id { get; set; }
        public string Nombre { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }

    public static class BancosWompiDefecto
    {
        public static List<BancoWompi> ObtenerListaDefecto()
        {
            return new List<BancoWompi>
            {
                new BancoWompi { Id = "1001", Nombre = "Bancolombia" },
                new BancoWompi { Id = "1002", Nombre = "Banco de Bogotá" },
                new BancoWompi { Id = "1003", Nombre = "Banco Popular" },
                new BancoWompi { Id = "1004", Nombre = "Banco de Occidente" },
                new BancoWompi { Id = "1006", Nombre = "Davivienda" },
                new BancoWompi { Id = "1007", Nombre = "BBVA Colombia" },
                new BancoWompi { Id = "1008", Nombre = "Banco Agrario" },
                new BancoWompi { Id = "1010", Nombre = "Colpatria" },
                new BancoWompi { Id = "1012", Nombre = "Banco AV Villas" },
                new BancoWompi { Id = "1013", Nombre = "Banco Caja Social" },
                new BancoWompi { Id = "1019", Nombre = "Banco Falabella" },
                new BancoWompi { Id = "1022", Nombre = "Banco Itaú" },
                new BancoWompi { Id = "1032", Nombre = "Banco GNB Sudameris" },
                new BancoWompi { Id = "1052", Nombre = "Nequi" },
            };
        }
    }
}
