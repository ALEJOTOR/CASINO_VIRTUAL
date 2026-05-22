using ENTITY;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class PartidaRepositorio : OracleBase<Partida>
    {
        public override IList<Partida> Consultar()
        {
            IList<Partida> lista = new List<Partida>();

            string sql = @"SELECT id_partida, id_usuario, id_juego,
                                  id_estado, fecha, apuesta,
                                  ganancia, resultado
                             FROM partidas
                            ORDER BY fecha DESC";

            using (OracleDataReader reader = EjecutarConsulta(sql))
                while (reader.Read())
                    lista.Add(Mapear(reader));

            return lista;
        }

        public IList<Partida> ObtenerPorUsuario(int idUsuario)
        {
            IList<Partida> lista = new List<Partida>();

            string sql = @"SELECT id_partida, id_usuario, id_juego,
                                  id_estado, fecha, apuesta,
                                  ganancia, resultado
                             FROM partidas
                            WHERE id_usuario = :id
                            ORDER BY fecha DESC";

            using (OracleDataReader reader = EjecutarConsulta(sql,
                new[] { (":id", (object)idUsuario) }))
                while (reader.Read())
                    lista.Add(Mapear(reader));

            return lista;
        }

        public override string Guardar(Partida p)
        {
            string sql = @"INSERT INTO partidas (
                               id_partida, id_usuario, id_juego,
                               id_estado, fecha, apuesta,
                               ganancia, resultado
                           ) VALUES (
                               seq_partidas.NEXTVAL, :id_usuario, :id_juego,
                               :id_estado, CURRENT_TIMESTAMP, :apuesta,
                               :ganancia, :resultado
                           )";

            return EjecutarComando(sql, new[]
            {
                (":id_usuario", (object)p.IdUsuario),
                (":id_juego",   (object)p.IdJuego),
                (":id_estado",  (object)p.IdEstado),
                (":apuesta",    (object)p.Apuesta),
                (":ganancia",   (object)p.Ganancia),
                (":resultado",  (object)(p.Resultado ?? (object)DBNull.Value))
            });
        }

        public string RegistrarConMovimientos(Partida p)
        {
            using (OracleConnection con = ConexionOracle.Abrir())
            using (OracleTransaction tx = con.BeginTransaction())
            {
                try
                {
                    decimal saldoActual = ConsultarSaldo(con, tx, p.IdUsuario, true);

                    if (saldoActual < p.Apuesta)
                        throw new InvalidOperationException("Saldo insuficiente.");

                    InsertarTransaccion(con, tx, p.IdUsuario, "perdida", p.Apuesta,
                        $"Apuesta partida juego {p.IdJuego}");
                    saldoActual = AplicarMovimientoSaldoSiHaceFalta(con, tx, p.IdUsuario, saldoActual, -p.Apuesta);

                    if (p.Resultado == "gano" && p.Ganancia > 0)
                    {
                        InsertarTransaccion(con, tx, p.IdUsuario, "ganancia", p.Ganancia,
                            $"Ganancia partida juego {p.IdJuego}");
                        saldoActual = AplicarMovimientoSaldoSiHaceFalta(con, tx, p.IdUsuario, saldoActual, p.Ganancia);
                    }

                    EjecutarComandoTransaccional(con, tx,
                        @"INSERT INTO partidas (
                              id_partida, id_usuario, id_juego,
                              id_estado, fecha, apuesta,
                              ganancia, resultado
                          ) VALUES (
                              seq_partidas.NEXTVAL, :id_usuario, :id_juego,
                              :id_estado, CURRENT_TIMESTAMP, :apuesta,
                              :ganancia, :resultado
                          )",
                        new[]
                        {
                            (":id_usuario", (object)p.IdUsuario),
                            (":id_juego", (object)p.IdJuego),
                            (":id_estado", (object)p.IdEstado),
                            (":apuesta", (object)p.Apuesta),
                            (":ganancia", (object)p.Ganancia),
                            (":resultado", (object)(p.Resultado ?? (object)DBNull.Value))
                        });

                    tx.Commit();
                    return "Guardado correctamente.";
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
                throw new InvalidOperationException("El saldo cambio inesperadamente durante la partida.");

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

        private void InsertarTransaccion(
            OracleConnection con,
            OracleTransaction tx,
            int idUsuario,
            string tipo,
            decimal monto,
            string descripcion)
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
                    (":tipo", (object)tipo),
                    (":monto", (object)monto),
                    (":descripcion", (object)(descripcion ?? (object)DBNull.Value))
                });
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

        private Partida Mapear(OracleDataReader r)
        {
            return new Partida
            {
                IdPartida = r.GetInt32(0),
                IdUsuario = r.GetInt32(1),
                IdJuego = r.GetInt32(2),
                IdEstado = r.GetInt32(3),
                Fecha = r.GetDateTime(4),
                Apuesta = r.GetDecimal(5),
                Ganancia = r.GetDecimal(6),
                Resultado = r.IsDBNull(7) ? null : r.GetString(7)
            };
        }
    }
}
