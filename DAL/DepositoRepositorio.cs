using ENTITY;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class DepositoRepositorio : OracleBase<Deposito>
    {
        public override IList<Deposito> Consultar()
        {
            return ObtenerPendientes();
        }

        public IList<Deposito> ObtenerPendientes()
        {
            IList<Deposito> lista = new List<Deposito>();

            using (OracleDataReader reader = EjecutarConsulta(
                "SELECT id_deposito, id_usuario, username, nombre, monto, estado, fecha_solicitud, fecha_procesamiento, metodo_pago, descripcion FROM vw_depositos_pendientes ORDER BY fecha_solicitud"))
                while (reader.Read())
                    lista.Add(Mapear(reader));

            return lista;
        }

        public IList<Deposito> ObtenerPorUsuario(int idUsuario)
        {
            IList<Deposito> lista = new List<Deposito>();

            using (OracleDataReader reader = EjecutarConsulta(
                @"SELECT d.id_deposito, d.id_usuario, u.username, u.nombre_1 || ' ' || u.apellido_1,
                         d.monto, d.estado, d.fecha_solicitud, d.fecha_procesamiento,
                         m.tipo, d.descripcion
                    FROM depositos d
                    JOIN usuarios u ON d.id_usuario = u.id_usuario
                    LEFT JOIN metodos_pago m ON d.id_metodo = m.id_metodo
                   WHERE d.id_usuario = :id
                   ORDER BY d.fecha_solicitud DESC",
                new[] { (":id", (object)idUsuario) }))
                while (reader.Read())
                    lista.Add(Mapear(reader));

            return lista;
        }

        public (string resultado, int idDeposito) Solicitar(int idUsuario, decimal monto, int? idMetodo, string descripcion)
        {
            var (resultado, idDeposito) = EjecutarSPConSalidaInt(
                "PKG_USUARIOS.pr_solicitar_deposito", "p_resultado", "p_id_deposito",
                ("p_id_usuario", idUsuario),
                ("p_monto", monto),
                ("p_id_metodo", (object)idMetodo ?? (object)DBNull.Value),
                ("p_descripcion", (object)descripcion ?? (object)DBNull.Value));

            return (resultado, idDeposito);
        }

        public string Confirmar(int idDeposito, string refWompi)
        {
            return EjecutarSP("PKG_USUARIOS.pr_confirmar_deposito", "p_resultado",
                ("p_id_deposito", idDeposito),
                ("p_referencia_wompi", (object)refWompi ?? (object)DBNull.Value));
        }

        public string Rechazar(int idDeposito, string motivo)
        {
            return EjecutarSP("PKG_USUARIOS.pr_rechazar_deposito", "p_resultado",
                ("p_id_deposito", idDeposito),
                ("p_motivo", (object)motivo ?? (object)DBNull.Value));
        }

        public void ActualizarReferenciaWompi(int idDeposito, string idLink)
        {
            EjecutarComando(
                "UPDATE depositos SET referencia_wompi = :ref WHERE id_deposito = :id",
                new[] { (":ref", (object)idLink ?? DBNull.Value), (":id", (object)idDeposito) });
        }

        public override string Guardar(Deposito entity)
        {
            throw new NotSupportedException("Use Solicitar() en lugar de Guardar().");
        }

        private Deposito Mapear(OracleDataReader r)
        {
            return new Deposito
            {
                IdDeposito = r.GetInt32(0),
                IdUsuario = r.GetInt32(1),
                Username = r.GetString(2),
                NombreUsuario = r.GetString(3),
                Monto = r.GetDecimal(4),
                Estado = r.GetString(5),
                FechaSolicitud = r.GetDateTime(6),
                FechaProcesamiento = r.IsDBNull(7) ? (DateTime?)null : r.GetDateTime(7),
                MetodoPago = r.IsDBNull(8) ? null : r.GetString(8),
                Descripcion = r.IsDBNull(9) ? null : r.GetString(9)
            };
        }
    }
}
