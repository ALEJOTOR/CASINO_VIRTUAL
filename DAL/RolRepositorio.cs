using ENTITY;
using System.Collections.Generic;
using System.IO;

namespace DAL
{
    public class RolRepositorio : ArchivoBase<Rol>
    {
        public RolRepositorio() : base(RutaArchivos.Roles) { }

        public override IList<Rol> Consultar()
        {
            IList<Rol> lista = new List<Rol>();
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

        private Rol Mapear(string linea)
        {
            // Formato: id|nombre_rol|descripcion
            string[] c = linea.Split('|');
            return new Rol
            {
                IdRol       = int.Parse(c[0]),
                NombreRol   = c[1],
                Descripcion = c[2]
            };
        }
    }
}
