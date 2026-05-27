using ENTITY;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class TransaccionRepositorio : OracleBase<Transaccion>
    {

        public override IList<Transaccion> Consultar()
        {
            IList<Transaccion> lista = new List<Transaccion>();

            string sql = @"SELECT id_transaccion, id_usuario, tipo,
                                  monto, fecha, descripcion
                             FROM transacciones
                            ORDER BY fecha DESC";

            using (OracleDataReader reader = EjecutarConsulta(sql))
                while (reader.Read())
                    lista.Add(Mapear(reader));

            return lista;
        }

        public IList<Transaccion> ObtenerPorUsuario(int idUsuario)
        {
            IList<Transaccion> lista = new List<Transaccion>();

            string sql = @"SELECT id_transaccion, id_usuario, tipo,
                                  monto, fecha, descripcion
                             FROM transacciones
                            WHERE id_usuario = :id
                            ORDER BY fecha DESC";

            using (OracleDataReader reader = EjecutarConsulta(sql,
                new[] { (":id", (object)idUsuario) }))
                while (reader.Read())
                    lista.Add(Mapear(reader));

            return lista;
        }

        public override string Guardar(Transaccion t)
        {
            EjecutarComando(@"INSERT INTO transacciones (
                                id_transaccion, id_usuario, tipo,
                                monto, fecha, descripcion
                            ) VALUES (
                                seq_transacciones.NEXTVAL, :id_usuario, :tipo,
                                :monto, CURRENT_TIMESTAMP, :descripcion
                            )", new[]
            {
                (":id_usuario",  (object)t.IdUsuario),
                (":tipo",        (object)t.Tipo),
                (":monto",       (object)t.Monto),
                (":descripcion", (object)(t.Descripcion ?? (object)DBNull.Value))
            });
            return "Guardado correctamente.";
        }

        public string RegistrarDepositoConSaldo(int idUsuario, decimal monto)
        {
            return EjecutarSP("PKG_USUARIOS.pr_realizar_deposito", "p_resultado",
                ("p_id_usuario", idUsuario),
                ("p_monto", monto));
        }

        private Transaccion Mapear(OracleDataReader r)
        {
            return new Transaccion
            {
                IdTransaccion = r.GetInt32(0),
                IdUsuario = r.GetInt32(1),
                Tipo = r.GetString(2),
                Monto = r.GetDecimal(3),
                Fecha = r.GetDateTime(4),
                Descripcion = r.IsDBNull(5) ? null : r.GetString(5)
            };
        }
    }
}