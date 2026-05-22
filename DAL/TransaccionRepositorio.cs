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
            string sql = @"INSERT INTO transacciones (
                               id_transaccion, id_usuario, tipo,
                               monto, fecha, descripcion
                           ) VALUES (
                               seq_transacciones.NEXTVAL, :id_usuario, :tipo,
                               :monto, CURRENT_TIMESTAMP, :descripcion
                           )";

            return EjecutarComando(sql, new[]
            {
                (":id_usuario",  (object)t.IdUsuario),
                (":tipo",        (object)t.Tipo),
                (":monto",       (object)t.Monto),
                (":descripcion", (object)(t.Descripcion ?? (object)DBNull.Value))
            });
        }

        public string RegistrarDepositoConSaldo(int idUsuario, decimal monto)
        {
            using (OracleConnection con = ConexionOracle.Abrir())
            using (OracleTransaction tx = con.BeginTransaction())
            {
                try
                {
                    EjecutarComandoTransaccional(con, tx,
                        @"INSERT INTO transacciones (
                              id_transaccion, id_usuario, tipo,
                              monto, fecha, descripcion
                          ) VALUES (
                              seq_transacciones.NEXTVAL, :id_usuario, :tipo,
                              :monto, CURRENT_TIMESTAMP, :descripcion
                          )",
                        new[]
                        {
                            (":id_usuario", (object)idUsuario),
                            (":tipo", (object)"deposito"),
                            (":monto", (object)monto),
                            (":descripcion", (object)"Recarga de saldo")
                        });

                    tx.Commit();
                    return "Deposito realizado correctamente.";
                }
                catch (Exception ex)
                {
                    try { tx.Rollback(); } catch { }
                    return ex.Message;
                }
            }
        }

        private int EjecutarComandoTransaccional(
            OracleConnection con,
            OracleTransaction tx,
            string sql,
            (string nombre, object valor)[] parametros)
        {
            using (OracleCommand cmd = new OracleCommand(sql, con))
            {
                cmd.Transaction = tx;
                cmd.BindByName = true;

                foreach (var (nombre, valor) in parametros)
                    cmd.Parameters.Add(new OracleParameter(nombre, valor));

                return cmd.ExecuteNonQuery();
            }
        }

        private Transaccion Mapear(OracleDataReader r)
        {
            return new Transaccion
            {
                IdTransaccion = r.GetInt32(0),
                IdUsuario = r.GetInt32(1),
                Tipo = r.GetString(2),
                Monto = r.GetDecimal(3),
                // Oracle devuelve TIMESTAMP como DateTime
                Fecha = r.GetDateTime(4),
                Descripcion = r.IsDBNull(5) ? null : r.GetString(5)
            };
        }
    }
}
