using ENTITY;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class UsuarioRepositorio : OracleBase<Usuario>
    {
        public override IList<Usuario> Consultar()
        {
            IList<Usuario> lista = new List<Usuario>();

            using (OracleDataReader reader = EjecutarConsulta(
                "SELECT * FROM vw_usuarios_detalle ORDER BY id_usuario"))
                while (reader.Read())
                    lista.Add(Mapear(reader));

            return lista;
        }

        public Usuario ObtenerPorUsername(string username)
        {
            using (OracleDataReader reader = EjecutarConsulta(
                "SELECT * FROM vw_usuarios_detalle WHERE username = :username",
                new[] { (":username", (object)username) }))
            {
                if (reader.Read())
                    return Mapear(reader);
                return null;
            }
        }

        public Usuario ObtenerPorId(int idUsuario)
        {
            using (OracleDataReader reader = EjecutarConsulta(
                "SELECT * FROM vw_usuarios_detalle WHERE id_usuario = :id",
                new[] { (":id", (object)idUsuario) }))
            {
                if (reader.Read())
                    return Mapear(reader);
                return null;
            }
        }

        public override string Guardar(Usuario u)
        {
            return EjecutarSP("PKG_USUARIOS.pr_registrar_usuario", "p_resultado",
                ("p_id_usuario", u.IdUsuario),
                ("p_username", u.Username),
                ("p_password", u.Password),
                ("p_nombre_1", u.Nombre1),
                ("p_nombre_2", (object)u.Nombre2 ?? DBNull.Value),
                ("p_apellido_1", u.Apellido1),
                ("p_apellido_2", (object)u.Apellido2 ?? DBNull.Value),
                ("p_correo", u.Correo),
                ("p_fecha_nac", u.FechaNacimiento),
                ("p_id_rol", u.IdRol));
        }

        public string Actualizar(Usuario u)
        {
            EjecutarComando(@"UPDATE usuarios SET
                                username         = :username,
                                password         = :password,
                                nombre_1         = :nombre1,
                                nombre_2         = :nombre2,
                                apellido_1       = :apellido1,
                                apellido_2       = :apellido2,
                                correo           = :correo,
                                fecha_nacimiento = TO_DATE(:fecha_nac, 'YYYY-MM-DD'),
                                id_rol           = :id_rol,
                                id_estado        = (SELECT id_estado FROM estado_usuario WHERE nombre = :estado)
                            WHERE id_usuario = :id", new[]
            {
                (":username",  (object)u.Username),
                (":password",  (object)u.Password),
                (":nombre1",   (object)u.Nombre1),
                (":nombre2",   (object)u.Nombre2   ?? DBNull.Value),
                (":apellido1", (object)u.Apellido1),
                (":apellido2", (object)u.Apellido2 ?? DBNull.Value),
                (":correo",    (object)u.Correo),
                (":fecha_nac", (object)u.FechaNacimiento.ToString("yyyy-MM-dd")),
                (":id_rol",    (object)u.IdRol),
                (":estado",    (object)(u.Estado ?? "activo")),
                (":id",        (object)u.IdUsuario)
            });
            return "Guardado correctamente.";
        }

        public string Eliminar(int idUsuario)
        {
            EjecutarComando(
                "DELETE FROM usuarios WHERE id_usuario = :id",
                new[] { (":id", (object)idUsuario) }
            );
            return "Guardado correctamente.";
        }

        private Usuario Mapear(OracleDataReader r)
        {
            return new Usuario
            {
                IdUsuario = r.GetInt32(0),
                Username = r.GetString(1),
                Password = r.GetString(2),
                Nombre1 = r.GetString(3),
                Nombre2 = r.IsDBNull(4) ? null : r.GetString(4),
                Apellido1 = r.GetString(5),
                Apellido2 = r.IsDBNull(6) ? null : r.GetString(6),
                Correo = r.GetString(7),
                FechaNacimiento = r.GetDateTime(8),
                Saldo = r.GetDecimal(9),
                IdRol = r.GetInt32(10),
                FechaRegistro = r.GetDateTime(11),
                Estado = r.GetString(12)
            };
        }
    }
}