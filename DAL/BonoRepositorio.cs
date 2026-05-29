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
                "SELECT id_bono, nombre, tipo, valor, descripcion, activo FROM bonos ORDER BY id_bono"))
                while (reader.Read())
                    lista.Add(Mapear(reader));

            return lista;
        }

        public IList<Bono> ObtenerActivos()
        {
            IList<Bono> lista = new List<Bono>();

            using (OracleDataReader reader = EjecutarConsulta(
                "SELECT id_bono, nombre, tipo, valor, descripcion, activo FROM bonos WHERE activo = 1 ORDER BY id_bono"))
                while (reader.Read())
                    lista.Add(Mapear(reader));

            return lista;
        }

        public Bono ObtenerPorTipo(string tipo)
        {
            using (OracleDataReader reader = EjecutarConsulta(
                "SELECT id_bono, nombre, tipo, valor, descripcion, activo FROM bonos WHERE tipo = :tipo AND activo = 1 AND ROWNUM = 1",
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
                "INSERT INTO bonos (id_bono, nombre, tipo, valor, descripcion, activo) VALUES (seq_bonos.NEXTVAL, :nombre, :tipo, :valor, :descripcion, :activo)",
                new[]
                {
                    (":nombre", (object)b.Nombre),
                    (":tipo", (object)b.Tipo),
                    (":valor", (object)b.Valor),
                    (":descripcion", (object)(b.Descripcion ?? (object)DBNull.Value)),
                    (":activo", (object)(b.Activo ? 1 : 0))
                });
            return "Guardado correctamente.";
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
                "SELECT id_usuario_bono, id_usuario, username, nombre_usuario, nombre_bono, tipo_bono, monto_aplicado, fecha_aplicado, estado, descripcion FROM vw_bonos_usuario WHERE id_usuario = :id ORDER BY fecha_aplicado DESC",
                new[] { (":id", (object)idUsuario) }))
                while (reader.Read())
                    lista.Add(MapearUsuarioBono(reader));

            return lista;
        }

        public IList<UsuarioBono> ObtenerTodosLosBonosAplicados()
        {
            IList<UsuarioBono> lista = new List<UsuarioBono>();

            using (OracleDataReader reader = EjecutarConsulta(
                "SELECT id_usuario_bono, id_usuario, username, nombre_usuario, nombre_bono, tipo_bono, monto_aplicado, fecha_aplicado, estado, descripcion FROM vw_bonos_usuario ORDER BY fecha_aplicado DESC"))
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
                Activo = r.GetInt32(5) == 1
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
                MontoAplicado = r.GetDecimal(6),
                FechaAplicado = r.GetDateTime(7),
                Estado = r.GetString(8),
                Descripcion = r.IsDBNull(9) ? null : r.GetString(9)
            };
        }
    }
}
