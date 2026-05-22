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

            string sql = @"SELECT u.id_usuario, u.username, u.password,
                                  u.nombre_1, u.nombre_2,
                                  u.apellido_1, u.apellido_2,
                                  u.correo, u.fecha_nacimiento,
                                  u.saldo, u.id_rol,
                                  u.fecha_registro, e.nombre AS estado
                             FROM usuarios u
                             JOIN estado_usuario e ON u.id_estado = e.id_estado
                            ORDER BY u.id_usuario";

            using (OracleDataReader reader = EjecutarConsulta(sql))
                while (reader.Read())
                    lista.Add(Mapear(reader));

            return lista;
        }

        public override string Guardar(Usuario u)
        {
            string sql = @"INSERT INTO usuarios (
                               id_usuario, username, password,
                               nombre_1, nombre_2,
                               apellido_1, apellido_2,
                               correo, fecha_nacimiento,
                               saldo, id_rol, id_estado, fecha_registro
                           ) VALUES (
                               seq_usuarios.NEXTVAL, :username, :password,
                               :nombre1, :nombre2,
                               :apellido1, :apellido2,
                               :correo, TO_DATE(:fecha_nac, 'YYYY-MM-DD'),
                               :saldo, :id_rol,
                               (SELECT id_estado FROM estado_usuario WHERE nombre = :estado),
                               SYSDATE
                           )";

            return EjecutarComando(sql, new[]
            {
                (":username",  (object)u.Username),
                (":password",  (object)u.Password),
                (":nombre1",   (object)u.Nombre1),
                // Si nombre2 o apellido2 son null, guardamos DBNull
                // para que Oracle guarde NULL y no el string "null"
                (":nombre2",   (object)u.Nombre2   ?? DBNull.Value),
                (":apellido1", (object)u.Apellido1),
                (":apellido2", (object)u.Apellido2 ?? DBNull.Value),
                (":correo",    (object)u.Correo),
                (":fecha_nac", (object)u.FechaNacimiento.ToString("yyyy-MM-dd")),
                (":saldo",     (object)u.Saldo),
                (":id_rol",    (object)u.IdRol),
                (":estado",    (object)(u.Estado ?? "activo"))
            });
        }
        public string Actualizar(Usuario u)
        {
            string sql = @"UPDATE usuarios SET
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
                           WHERE id_usuario = :id";

            return EjecutarComando(sql, new[]
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
        }
        public string Eliminar(int idUsuario)
        {
            return EjecutarComando(
                "DELETE FROM usuarios WHERE id_usuario = :id",
                new[] { (":id", (object)idUsuario) }
            );
        }

        public Usuario ObtenerPorUsername(string username)
        {
            string sql = @"SELECT u.id_usuario, u.username, u.password,
                                  u.nombre_1, u.nombre_2,
                                  u.apellido_1, u.apellido_2,
                                  u.correo, u.fecha_nacimiento,
                                  u.saldo, u.id_rol,
                                  u.fecha_registro, e.nombre AS estado
                             FROM usuarios u
                             JOIN estado_usuario e ON u.id_estado = e.id_estado
                            WHERE u.username = :username";

            using (OracleDataReader reader = EjecutarConsulta(sql,
                new[] { (":username", (object)username) }))
            {
                if (reader.Read())
                    return Mapear(reader);
                return null;
            }
        }

        public Usuario ObtenerPorId(int idUsuario)
        {
            string sql = @"SELECT u.id_usuario, u.username, u.password,
                                  u.nombre_1, u.nombre_2,
                                  u.apellido_1, u.apellido_2,
                                  u.correo, u.fecha_nacimiento,
                                  u.saldo, u.id_rol,
                                  u.fecha_registro, e.nombre AS estado
                             FROM usuarios u
                             JOIN estado_usuario e ON u.id_estado = e.id_estado
                            WHERE u.id_usuario = :id";

            using (OracleDataReader reader = EjecutarConsulta(sql,
                new[] { (":id", (object)idUsuario) }))
            {
                if (reader.Read())
                    return Mapear(reader);
                return null;
            }
        }

        private Usuario Mapear(OracleDataReader r)
        {
            return new Usuario
            {
                IdUsuario = r.GetInt32(0),
                Username = r.GetString(1),
                Password = r.GetString(2),
                Nombre1 = r.GetString(3),
                // IsDBNull verifica si Oracle devolvió NULL para
                // campos opcionales (nombre_2, apellido_2)
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
