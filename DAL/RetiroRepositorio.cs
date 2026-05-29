using ENTITY;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class RetiroRepositorio : OracleBase<Retiro>
    {
        public override IList<Retiro> Consultar()
        {
            return ObtenerPendientes();
        }

        public IList<Retiro> ObtenerPendientes()
        {
            IList<Retiro> lista = new List<Retiro>();

            using (OracleDataReader reader = EjecutarConsulta(
                "SELECT id_retiro, id_usuario, username, nombre, monto, estado, fecha_solicitud, fecha_procesamiento, metodo_pago, admin_revisor FROM vw_retiros_pendientes ORDER BY fecha_solicitud"))
                while (reader.Read())
                    lista.Add(Mapear(reader));

            return lista;
        }

        public IList<Retiro> ObtenerPorUsuario(int idUsuario)
        {
            IList<Retiro> lista = new List<Retiro>();

            using (OracleDataReader reader = EjecutarConsulta(
                @"SELECT r.id_retiro, r.id_usuario, u.username, u.nombre_1 || ' ' || u.apellido_1,
                         r.monto, r.estado, r.fecha_solicitud, r.fecha_procesamiento,
                         m.tipo, ua.username
                    FROM retiros r
                    JOIN usuarios u ON r.id_usuario = u.id_usuario
                    LEFT JOIN metodos_pago m ON r.id_metodo = m.id_metodo
                    LEFT JOIN usuarios ua ON r.id_admin_revisor = ua.id_usuario
                   WHERE r.id_usuario = :id
                   ORDER BY r.fecha_solicitud DESC",
                new[] { (":id", (object)idUsuario) }))
                while (reader.Read())
                    lista.Add(Mapear(reader));

            return lista;
        }

        public (string resultado, int idRetiro) Solicitar(int idUsuario, decimal monto, int? idMetodo)
        {
            var (resultado, idRetiro) = EjecutarSPConSalidaInt(
                "PKG_USUARIOS.pr_solicitar_retiro", "p_resultado", "p_id_retiro",
                ("p_id_usuario", idUsuario),
                ("p_monto", monto),
                ("p_id_metodo", (object)idMetodo ?? DBNull.Value));

            return (resultado, idRetiro);
        }

        public string Aprobar(int idRetiro, int idAdmin, string refWompi)
        {
            return EjecutarSP("PKG_USUARIOS.pr_aprobar_retiro", "p_resultado",
                ("p_id_retiro", idRetiro),
                ("p_id_admin", idAdmin),
                ("p_referencia_wompi", (object)refWompi ?? (object)DBNull.Value));
        }

        public string Rechazar(int idRetiro, string motivo)
        {
            return EjecutarSP("PKG_USUARIOS.pr_rechazar_retiro", "p_resultado",
                ("p_id_retiro", idRetiro),
                ("p_motivo", (object)motivo ?? (object)DBNull.Value));
        }

        public override string Guardar(Retiro entity)
        {
            throw new NotSupportedException("Use Solicitar() en lugar de Guardar().");
        }

        private Retiro Mapear(OracleDataReader r)
        {
            return new Retiro
            {
                IdRetiro = r.GetInt32(0),
                IdUsuario = r.GetInt32(1),
                Username = r.GetString(2),
                NombreUsuario = r.GetString(3),
                Monto = r.GetDecimal(4),
                Estado = r.GetString(5),
                FechaSolicitud = r.GetDateTime(6),
                FechaProcesamiento = r.IsDBNull(7) ? (DateTime?)null : r.GetDateTime(7),
                MetodoPago = r.IsDBNull(8) ? null : r.GetString(8),
                AdminRevisor = r.IsDBNull(9) ? null : r.GetString(9)
            };
        }
    }
}
