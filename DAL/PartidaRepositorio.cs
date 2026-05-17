using ENTITY;
using System;
using System.Collections.Generic;
using System.IO;

namespace DAL
{
    public class PartidaRepositorio : ArchivoBase<Partida>
    {
        public PartidaRepositorio() : base(RutaArchivos.Partidas) { }

        public override IList<Partida> Consultar()
        {
            IList<Partida> lista = new List<Partida>();
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

        private Partida Mapear(string linea)
        {
            // Formato: id|idUsuario|idJuego|idEstado|fecha|apuesta|ganancia|resultado
            string[] c = linea.Split('|');
            return new Partida
            {
                IdPartida = int.Parse(c[0]),
                IdUsuario = int.Parse(c[1]),
                IdJuego   = int.Parse(c[2]),
                IdEstado  = int.Parse(c[3]),
                Fecha     = DateTime.Parse(c[4]),
                Apuesta   = decimal.Parse(c[5]),
                Ganancia  = decimal.Parse(c[6]),
                Resultado = c[7]
            };
        }
    }
}
