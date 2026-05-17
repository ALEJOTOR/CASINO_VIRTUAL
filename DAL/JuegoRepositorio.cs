using ENTITY;
using System.Collections.Generic;
using System.IO;

namespace DAL
{
    public class JuegoRepositorio : ArchivoBase<Juego>
    {
        public JuegoRepositorio() : base(RutaArchivos.Juegos) { }

        public override IList<Juego> Consultar()
        {
            IList<Juego> lista = new List<Juego>();
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

        private Juego Mapear(string linea)
        {
            // Formato: id|nombre|descripcion|estado
            string[] c = linea.Split('|');
            return new Juego
            {
                IdJuego     = int.Parse(c[0]),
                Nombre      = c[1],
                Descripcion = c[2],
                Estado      = c[3]
            };
        }
    }
}
