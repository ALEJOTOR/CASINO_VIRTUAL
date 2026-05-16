using System;
using System.Collections.Generic;
using System.IO;
using ENTITY;

namespace DAL
{
    // Formato: id|username|password|nombre1|nombre2|apellido1|apellido2|correo|fechaNac|saldo|idRol|fechaRegistro|estado
    public class UsuarioDAL
    {
        private Usuario ParsearLinea(string linea)
        {
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

        private string SerializarLinea(Usuario u)
        {
            return string.Join("|",
                u.IdUsuario,
                u.Username,
                u.Password,
                u.Nombre1,
                u.Nombre2 ?? "",
                u.Apellido1,
                u.Apellido2 ?? "",
                u.Correo,
                u.FechaNacimiento.ToString("yyyy-MM-dd"),
                u.Saldo.ToString("F2"),
                u.IdRol,
                u.FechaRegistro.ToString("yyyy-MM-dd"),
                u.Estado
            );
        }

        public List<Usuario> ConsultarTodos()
        {
            var lista = new List<Usuario>();
            if (!File.Exists(RutaArchivos.Usuarios)) return lista;

            using (StreamReader sr = new StreamReader(RutaArchivos.Usuarios))
            {
                string linea;
                while ((linea = sr.ReadLine()) != null)
                    if (!string.IsNullOrWhiteSpace(linea))
                        lista.Add(ParsearLinea(linea));
            }
            return lista;
        }

        public Usuario ConsultarPorId(int id)
        {
            foreach (var u in ConsultarTodos())
                if (u.IdUsuario == id) return u;
            return null;
        }

        public Usuario Login(string username, string password)
        {
            foreach (var u in ConsultarTodos())
                if (u.Username == username && u.Password == password && u.Estado == "activo")
                    return u;
            return null;
        }

        private int SiguienteId()
        {
            var lista = ConsultarTodos();
            return lista.Count == 0 ? 1 : lista[lista.Count - 1].IdUsuario + 1;
        }

        public bool Insertar(Usuario u)
        {
            RutaArchivos.CrearCarpeta();
            u.IdUsuario     = SiguienteId();
            u.FechaRegistro = DateTime.Now;
            u.Estado        = u.Estado ?? "activo";

            using (StreamWriter sw = new StreamWriter(RutaArchivos.Usuarios, append: true))
                sw.WriteLine(SerializarLinea(u));
            return true;
        }

        public bool Actualizar(Usuario uActualizado)
        {
            var lista = ConsultarTodos();
            bool encontrado = false;

            using (StreamWriter sw = new StreamWriter(RutaArchivos.Usuarios, append: false))
            {
                foreach (var u in lista)
                {
                    if (u.IdUsuario == uActualizado.IdUsuario)
                    {
                        sw.WriteLine(SerializarLinea(uActualizado));
                        encontrado = true;
                    }
                    else
                    {
                        sw.WriteLine(SerializarLinea(u));
                    }
                }
            }
            return encontrado;
        }

        public bool Eliminar(int id)
        {
            var lista = ConsultarTodos();
            bool encontrado = false;

            using (StreamWriter sw = new StreamWriter(RutaArchivos.Usuarios, append: false))
            {
                foreach (var u in lista)
                {
                    if (u.IdUsuario == id) { encontrado = true; continue; }
                    sw.WriteLine(SerializarLinea(u));
                }
            }
            return encontrado;
        }

        public bool ActualizarSaldo(int idUsuario, decimal nuevoSaldo)
        {
            var u = ConsultarPorId(idUsuario);
            if (u == null) return false;
            u.Saldo = nuevoSaldo;
            return Actualizar(u);
        }

        public void InicializarAdmin()
        {
            if (File.Exists(RutaArchivos.Usuarios) && new FileInfo(RutaArchivos.Usuarios).Length > 0)
                return;

            // Admin por defecto: usuario=admin, password=admin123 (ya hasheado)
            Insertar(new Usuario
            {
                Username        = "admin",
                Password        = "6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b",
                Nombre1         = "Admin",
                Apellido1       = "Casino",
                Correo          = "admin@casino.com",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Saldo           = 0,
                IdRol           = 1,
                Estado          = "activo"
            });
        }
    }
}
