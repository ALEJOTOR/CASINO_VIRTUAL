using ENTITY;
using System;
using System.Collections.Generic;
using System.IO;

namespace DAL
{
    public class TransaccionRepositorio : ArchivoBase<Transaccion>
    {
        public TransaccionRepositorio() : base(RutaArchivos.Transacciones) { }

        public override IList<Transaccion> Consultar()
        {
            IList<Transaccion> lista = new List<Transaccion>();
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

        private Transaccion Mapear(string linea)
        {
            // Formato: id|idUsuario|tipo|monto|fecha|descripcion
            string[] c = linea.Split('|');
            return new Transaccion
            {
                IdTransaccion = int.Parse(c[0]),
                IdUsuario     = int.Parse(c[1]),
                Tipo          = c[2],
                Monto         = decimal.Parse(c[3]),
                Fecha         = DateTime.Parse(c[4]),
                Descripcion   = c[5]
            };
        }
    }
}
