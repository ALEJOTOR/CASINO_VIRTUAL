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
            return EjecutarSP("pr_actualizar_usuario", "p_resultado",
                ("p_id_usuario", u.IdUsuario),
                ("p_username", u.Username),
                ("p_password", u.Password),
                ("p_nombre_1", u.Nombre1),
                ("p_nombre_2", (object)u.Nombre2 ?? DBNull.Value),
                ("p_apellido_1", u.Apellido1),
                ("p_apellido_2", (object)u.Apellido2 ?? DBNull.Value),
                ("p_correo", u.Correo),
                ("p_fecha_nac", u.FechaNacimiento),
                ("p_id_rol", u.IdRol),
                ("p_estado", (object)(u.Estado ?? "activo")));
        }

        public string CambiarEstado(int idUsuario, string nuevoEstado)
        {
            return EjecutarSP("pr_cambiar_estado", "p_resultado",
                ("p_id_usuario", idUsuario),
                ("p_nuevo_estado", nuevoEstado));
        }

        public decimal ObtenerTotalDepositado(int idUsuario)
        {
            return EjecutarScalar<decimal>(
                "SELECT PKG_USUARIOS.fn_total_depositado(:p_id) FROM dual",
                new[] { (":p_id", (object)idUsuario) });
        }

        public decimal ObtenerGananciaNeta(int idUsuario)
        {
            return EjecutarScalar<decimal>(
                "SELECT PKG_USUARIOS.fn_calcular_ganancia_neta(:p_id) FROM dual",
                new[] { (":p_id", (object)idUsuario) });
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