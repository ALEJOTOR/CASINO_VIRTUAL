using System;
using System.Collections.Generic;
using System.IO;
using ENTITY;

namespace DAL
{
    // Formato de línea: id|nombre_rol|descripcion
    public class RolDAL
    {
        public List<Rol> ConsultarTodos()
        {
            var lista = new List<Rol>();
            if (!File.Exists(RutaArchivos.Roles)) return lista;

            using (StreamReader sr = new StreamReader(RutaArchivos.Roles))
            {
                string linea;
                while ((linea = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(linea)) continue;
                    string[] campos = linea.Split('|');
                    lista.Add(new Rol
                    {
                        IdRol      = int.Parse(campos[0]),
                        NombreRol  = campos[1],
                        Descripcion = campos[2]
                    });
                }
            }
            return lista;
        }

        public Rol ConsultarPorId(int id)
        {
            foreach (var r in ConsultarTodos())
                if (r.IdRol == id) return r;
            return null;
        }

        // Genera el siguiente ID disponible
        private int SiguienteId()
        {
            var lista = ConsultarTodos();
            return lista.Count == 0 ? 1 : lista[lista.Count - 1].IdRol + 1;
        }

        public bool Insertar(Rol r)
        {
            RutaArchivos.CrearCarpeta();
            r.IdRol = SiguienteId();
            using (StreamWriter sw = new StreamWriter(RutaArchivos.Roles, append: true))
                sw.WriteLine($"{r.IdRol}|{r.NombreRol}|{r.Descripcion}");
            return true;
        }

        /// <summary>Carga datos iniciales si el archivo está vacío.</summary>
        public void InicializarDatos()
        {
            if (File.Exists(RutaArchivos.Roles) && new FileInfo(RutaArchivos.Roles).Length > 0)
                return;

            RutaArchivos.CrearCarpeta();
            Insertar(new Rol { NombreRol = "administrador", Descripcion = "Acceso total al sistema" });
            Insertar(new Rol { NombreRol = "cliente",       Descripcion = "Acceso a juegos y saldo" });
        }
    }
}
