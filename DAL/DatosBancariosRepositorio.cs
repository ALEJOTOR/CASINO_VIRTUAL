using ENTITY;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class DatosBancariosRepositorio : OracleBase<DatosBancarios>
    {
        public override IList<DatosBancarios> Consultar()
        {
            throw new NotSupportedException("Use ObtenerPorUsuario en su lugar.");
        }

        public override string Guardar(DatosBancarios entity)
        {
            return EjecutarSP("PKG_USUARIOS.pr_guardar_datos_bancarios", "p_resultado",
                ("p_id_usuario", entity.IdUsuario),
                ("p_banco_id", entity.BancoId),
                ("p_banco_nombre", entity.BancoNombre),
                ("p_tipo_cuenta", entity.TipoCuenta),
                ("p_numero_cuenta", entity.NumeroCuenta),
                ("p_tipo_doc", entity.TipoDoc),
                ("p_numero_doc", entity.NumeroDoc),
                ("p_nombre_titular", entity.NombreTitular));
        }

        public IList<DatosBancarios> ObtenerPorUsuario(int idUsuario)
        {
            IList<DatosBancarios> lista = new List<DatosBancarios>();

            using (OracleDataReader reader = EjecutarConsulta(
                @"SELECT id_datos_bancarios, id_usuario, username, banco_id, banco_nombre,
                         tipo_cuenta, numero_cuenta, tipo_doc, numero_doc,
                         nombre_titular, activo, fecha_registro
                   FROM vw_datos_bancarios
                   WHERE id_usuario = :id
                   ORDER BY fecha_registro DESC",
                new[] { (":id", (object)idUsuario) }))
                while (reader.Read())
                    lista.Add(Mapear(reader));

            return lista;
        }

        public DatosBancarios ObtenerActivoPorUsuario(int idUsuario)
        {
            using (OracleDataReader reader = EjecutarConsulta(
                @"SELECT id_datos_bancarios, id_usuario, username, banco_id, banco_nombre,
                         tipo_cuenta, numero_cuenta, tipo_doc, numero_doc,
                         nombre_titular, activo, fecha_registro
                   FROM vw_datos_bancarios
                   WHERE id_usuario = :id AND activo = 1",
                new[] { (":id", (object)idUsuario) }))
                if (reader.Read())
                    return Mapear(reader);

            return null;
        }

        private DatosBancarios Mapear(OracleDataReader r)
        {
            return new DatosBancarios
            {
                IdDatosBancarios = r.GetInt32(0),
                IdUsuario = r.GetInt32(1),
                BancoId = r.GetString(3),
                BancoNombre = r.IsDBNull(4) ? null : r.GetString(4),
                TipoCuenta = r.GetString(5),
                NumeroCuenta = r.GetString(6),
                TipoDoc = r.GetString(7),
                NumeroDoc = r.GetString(8),
                NombreTitular = r.GetString(9),
                Activo = r.GetInt32(10) == 1,
                FechaRegistro = r.GetDateTime(11)
            };
        }
    }
}
