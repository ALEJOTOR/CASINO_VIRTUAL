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
                    decimal saldoInicial = ConsultarSaldo(con, tx, idUsuario, true);

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

                    AplicarMovimientoSaldoSiHaceFalta(con, tx, idUsuario, saldoInicial, monto);

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

        private decimal AplicarMovimientoSaldoSiHaceFalta(
            OracleConnection con,
            OracleTransaction tx,
            int idUsuario,
            decimal saldoAnterior,
            decimal movimiento)
        {
            decimal saldoDespuesTransaccion = ConsultarSaldo(con, tx, idUsuario, false);
            decimal saldoEsperado = saldoAnterior + movimiento;

            if (saldoDespuesTransaccion == saldoEsperado)
                return saldoDespuesTransaccion;

            if (saldoDespuesTransaccion != saldoAnterior)
                throw new InvalidOperationException("El saldo cambio inesperadamente durante el deposito.");

            int filas = EjecutarComandoTransaccional(con, tx,
                @"UPDATE usuarios
                     SET saldo = saldo + :movimiento
                   WHERE id_usuario = :id_usuario",
                new[]
                {
                    (":movimiento", (object)movimiento),
                    (":id_usuario", (object)idUsuario)
                });

            if (filas == 0)
                throw new InvalidOperationException("Usuario no encontrado.");

            return saldoEsperado;
        }

        private decimal ConsultarSaldo(
            OracleConnection con,
            OracleTransaction tx,
            int idUsuario,
            bool bloquear)
        {
            string sql = bloquear
                ? "SELECT saldo FROM usuarios WHERE id_usuario = :id_usuario FOR UPDATE"
                : "SELECT saldo FROM usuarios WHERE id_usuario = :id_usuario";

            using (OracleCommand cmd = new OracleCommand(sql, con))
            {
                cmd.Transaction = tx;
                cmd.BindByName = true;
                cmd.Parameters.Add(new OracleParameter(":id_usuario", idUsuario));

                object resultado = cmd.ExecuteScalar();
                if (resultado == null || resultado == DBNull.Value)
                    throw new InvalidOperationException("Usuario no encontrado.");

                return Convert.ToDecimal(resultado);
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
