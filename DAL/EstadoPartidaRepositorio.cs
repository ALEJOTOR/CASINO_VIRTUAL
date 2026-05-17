using ENTITY;
using System.Collections.Generic;
using System.IO;

namespace DAL
{
    public class EstadoPartidaRepositorio : ArchivoBase<EstadoPartida>
    {
        public EstadoPartidaRepositorio() : base(RutaArchivos.EstadoPartidas) { }

        public override IList<EstadoPartida> Consultar()
        {
            IList<EstadoPartida> lista = new List<EstadoPartida>();
            if (!File.Exists(_nombreArchivo)) return lista;

            StreamReader lector = new StreamReader(_nombreArchivo);
            while (!lector.EndOfStream)
            {
                string linea = lector.ReadLine();
                if (!string.IsNullOrWhiteSpace(linea))
                    lista.Add(Mapear(linea));
            }
            lector.Close();
            return lista;
        }

        private EstadoPartida Mapear(string linea)
        {
            // Formato: id|nombre_estado|descripcion
            string[] c = linea.Split('|');
            return new EstadoPartida
            {
                IdEstado     = int.Parse(c[0]),
                NombreEstado = c[1],
                Descripcion  = c[2]
            };
        }
    }
}
