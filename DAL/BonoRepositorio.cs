using ENTITY;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class BonoRepositorio : OracleBase<Bono>
    {
        public override IList<Bono> Consultar()
        {
            IList<Bono> lista = new List<Bono>();

            using (OracleDataReader reader = EjecutarConsulta(
                "SELECT id_bono, nombre, tipo, valor, descripcion, activo, fecha_inicio, fecha_fin, usos_maximos, usos_actuales FROM bonos ORDER BY id_bono"))
                while (reader.Read())
                    lista.Add(Mapear(reader));

            return lista;
        }

        public IList<Bono> ObtenerActivos()
        {
            IList<Bono> lista = new List<Bono>();

            using (OracleDataReader reader = EjecutarConsulta(
                "SELECT id_bono, nombre, tipo, valor, descripcion, activo, fecha_inicio, fecha_fin, usos_maximos, usos_actuales FROM bonos WHERE activo = 1 ORDER BY id_bono"))
                while (reader.Read())
                    lista.Add(Mapear(reader));

            return lista;
        }

        public Bono ObtenerPorTipo(string tipo)
        {
            using (OracleDataReader reader = EjecutarConsulta(
                "SELECT id_bono, nombre, tipo, valor, descripcion, activo, fecha_inicio, fecha_fin, usos_maximos, usos_actuales FROM bonos WHERE tipo = :tipo AND activo = 1 AND ROWNUM = 1",
                new[] { (":tipo", (object)tipo) }))
            {
                if (reader.Read())
                    return Mapear(reader);
                return null;
            }
        }

        public override string Guardar(Bono b)
        {
            EjecutarComando(
                "INSERT INTO bonos (id_bono, nombre, tipo, valor, descripcion, activo, fecha_inicio, fecha_fin, usos_maximos, usos_actuales) VALUES (seq_bonos.NEXTVAL, :nombre, :tipo, :valor, :descripcion, :activo, :fecha_inicio, :fecha_fin, :usos_maximos, :usos_actuales)",
                new[]
                {
                    (":nombre", (object)b.Nombre),
                    (":tipo", (object)b.Tipo),
                    (":valor", (object)b.Valor),
                    (":descripcion", (object)(b.Descripcion ?? (object)DBNull.Value)),
                    (":activo", (object)(b.Activo ? 1 : 0)),
                    (":fecha_inicio", (object)b.FechaInicio),
                    (":fecha_fin", (object)b.FechaFin ?? (object)DBNull.Value),
                    (":usos_maximos", (object)b.UsosMaximos ?? (object)DBNull.Value),
                    (":usos_actuales", (object)b.UsosActuales)
                });
            return "Guardado correctamente.";
        }

        public string Actualizar(Bono b)
        {
            EjecutarComando(
                "UPDATE bonos SET nombre = :nombre, tipo = :tipo, valor = :valor, descripcion = :descripcion, activo = :activo, fecha_inicio = :fecha_inicio, fecha_fin = :fecha_fin, usos_maximos = :usos_maximos WHERE id_bono = :id_bono",
                new[]
                {
                    (":nombre", (object)b.Nombre),
                    (":tipo", (object)b.Tipo),
                    (":valor", (object)b.Valor),
                    (":descripcion", (object)(b.Descripcion ?? (object)DBNull.Value)),
                    (":activo", (object)(b.Activo ? 1 : 0)),
                    (":fecha_inicio", (object)b.FechaInicio),
                    (":fecha_fin", (object)b.FechaFin ?? (object)DBNull.Value),
                    (":usos_maximos", (object)b.UsosMaximos ?? (object)DBNull.Value),
                    (":id_bono", (object)b.IdBono)
                });
            return "Guardado correctamente.";
        }

        public string Desactivar(int idBono)
        {
            EjecutarComando("UPDATE bonos SET activo = 0 WHERE id_bono = :id", new[] { (":id", (object)idBono) });
            return "Guardado correctamente.";
        }

        public string RevocarBono(int idUsuarioBono, string motivo)
        {
            return EjecutarSP("PR_REVOCAR_BONO", "p_resultado",
                ("p_id_usuario_bono", (object)idUsuarioBono),
                ("p_motivo", (object)(motivo ?? (object)DBNull.Value)));
        }

        public (int totalBonos, int bonosActivos, decimal montoTotal, int usuariosConBono) ObtenerResumen()
        {
            string sql = @"SELECT (SELECT COUNT(*) FROM bonos) total_bonos,
(SELECT COUNT(*) FROM bonos WHERE activo = 1) bonos_activos,
(SELECT NVL(SUM(monto_aplicado),0) FROM usuario_bonos) monto_total,
(SELECT COUNT(DISTINCT id_usuario) FROM usuario_bonos) usuarios_con_bono FROM DUAL";
            using (OracleDataReader reader = EjecutarConsulta(sql))
            {
                if (reader.Read())
                    return (reader.GetInt32(0), reader.GetInt32(1), reader.GetDecimal(2), reader.GetInt32(3));
            }
            return (0, 0, 0, 0);
        }

        public string AplicarBono(int idUsuario, int idBono, decimal monto, string descripcion)
        {
            return EjecutarSP("PKG_USUARIOS.pr_aplicar_bono", "p_resultado",
                ("p_id_usuario", idUsuario),
                ("p_id_bono", idBono),
                ("p_monto", monto),
                ("p_descripcion", (object)descripcion ?? (object)DBNull.Value));
        }

        public IList<UsuarioBono> ObtenerBonosPorUsuario(int idUsuario)
        {
            IList<UsuarioBono> lista = new List<UsuarioBono>();

            using (OracleDataReader reader = EjecutarConsulta(
                "SELECT id_usuario_bono, id_usuario, username, nombre_usuario, nombre_bono, tipo_bono, valor_bono, monto_aplicado, fecha_aplicado, fecha_expiracion, estado, descripcion, bono_consumido, estado_vigencia FROM vw_bonos_usuario WHERE id_usuario = :id ORDER BY fecha_aplicado DESC",
                new[] { (":id", (object)idUsuario) }))
                while (reader.Read())
                    lista.Add(MapearUsuarioBono(reader));

            return lista;
        }

        public IList<UsuarioBono> ObtenerTodosLosBonosAplicados()
        {
            IList<UsuarioBono> lista = new List<UsuarioBono>();

            using (OracleDataReader reader = EjecutarConsulta(
                "SELECT id_usuario_bono, id_usuario, username, nombre_usuario, nombre_bono, tipo_bono, valor_bono, monto_aplicado, fecha_aplicado, fecha_expiracion, estado, descripcion, bono_consumido, estado_vigencia FROM vw_bonos_usuario ORDER BY fecha_aplicado DESC"))
                while (reader.Read())
                    lista.Add(MapearUsuarioBono(reader));

            return lista;
        }

        private Bono Mapear(OracleDataReader r)
        {
            return new Bono
            {
                IdBono = r.GetInt32(0),
                Nombre = r.GetString(1),
                Tipo = r.GetString(2),
                Valor = r.GetDecimal(3),
                Descripcion = r.IsDBNull(4) ? null : r.GetString(4),
                Activo = r.GetInt32(5) == 1,
                FechaInicio = r.IsDBNull(6) ? DateTime.Now : r.GetDateTime(6),
                FechaFin = r.IsDBNull(7) ? (DateTime?)null : r.GetDateTime(7),
                UsosMaximos = r.IsDBNull(8) ? (int?)null : r.GetInt32(8),
                UsosActuales = r.IsDBNull(9) ? 0 : r.GetInt32(9)
            };
        }

        private UsuarioBono MapearUsuarioBono(OracleDataReader r)
        {
            return new UsuarioBono
            {
                IdUsuarioBono = r.GetInt32(0),
                IdUsuario = r.GetInt32(1),
                Username = r.GetString(2),
                NombreUsuario = r.GetString(3),
                NombreBono = r.GetString(4),
                TipoBono = r.GetString(5),
                ValorBono = r.IsDBNull(6) ? 0 : r.GetDecimal(6),
                MontoAplicado = r.IsDBNull(7) ? 0 : r.GetDecimal(7),
                FechaAplicado = r.GetDateTime(8),
                FechaExpiracion = r.IsDBNull(9) ? (DateTime?)null : r.GetDateTime(9),
                Estado = r.GetString(10),
                Descripcion = r.IsDBNull(11) ? null : r.GetString(11),
                BonoConsumido = r.IsDBNull(12) ? false : r.GetInt32(12) == 1,
                EstadoVigencia = r.IsDBNull(13) ? "Permanente" : r.GetString(13)
            };
        }
        public void ActualizarFechaExpiracion(int idUsuario, int idBono, DateTime fechaExpiracion)
        {
            EjecutarComando(
                "UPDATE usuario_bonos SET fecha_expiracion = :fecha WHERE id_usuario_bono = (SELECT MAX(id_usuario_bono) FROM usuario_bonos WHERE id_usuario = :uid AND id_bono = :bid)",
                new[] {
                    (":fecha", (object)fechaExpiracion),
                    (":uid", (object)idUsuario),
                    (":bid", (object)idBono)
                });
        }
    }
}
