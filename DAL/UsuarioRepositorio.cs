using ENTITY;
using System;
using System.Collections.Generic;
using System.IO;

namespace DAL
{
    public class UsuarioRepositorio : ArchivoBase<Usuario>
    {
        public UsuarioRepositorio() : base(RutaArchivos.Usuarios) { }

        public override IList<Usuario> Consultar()
        {
            IList<Usuario> lista = new List<Usuario>();
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

        private Usuario Mapear(string linea)
        {
            // Formato: id|username|password|nombre1|nombre2|apellido1|apellido2|correo|fechaNac|saldo|idRol|fechaRegistro|estado
            string[] c = linea.Split('|');
            return new Usuario
            {
                IdUsuario       = int.Parse(c[0]),
                Username        = c[1],
                Password        = c[2],
                Nombre1         = c[3],
                Nombre2         = c[4],
                Apellido1       = c[5],
                Apellido2       = c[6],
                Correo          = c[7],
                FechaNacimiento = DateTime.Parse(c[8]),
                Saldo           = decimal.Parse(c[9]),
                IdRol           = int.Parse(c[10]),
                FechaRegistro   = DateTime.Parse(c[11]),
                Estado          = c[12]
            };
        }
    }
}
