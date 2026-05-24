using ENTITY;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public class PartidaRepositorio : OracleBase<Partida>
    {
        // ── CONSULTAS — no cambian, siguen contra la tabla directa ────────────

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

        // ── HISTORIAL CON VISTA — nuevo método usando vw_historial_partidas ───
        // A diferencia de ObtenerPorUsuario (que devuelve IDs crudos),
        // este método devuelve nombres legibles directamente desde la vista:
        // nombre del juego ("Minas") y estado ("ganada", "perdida"), etc.
        // Úsalo en la pantalla de historial del cliente.

        public IList<HistorialPartida> ObtenerHistorialPorUsuario(int idUsuario)
        {
            IList<HistorialPartida> lista = new List<HistorialPartida>();

            string sql = @"SELECT id_partida, id_usuario, nombre_juego,
                                  estado, fecha, apuesta,
                                  ganancia, resultado
                             FROM vw_historial_partidas
                            WHERE id_usuario = :id
                            ORDER BY fecha DESC";

            using (OracleDataReader r = EjecutarConsulta(sql,
                new[] { (":id", (object)idUsuario) }))
                while (r.Read())
                    lista.Add(new HistorialPartida
                    {
                        IdPartida = r.GetInt32(0),
                        IdUsuario = r.GetInt32(1),
                        NombreJuego = r.GetString(2),
                        Estado = r.GetString(3),
                        Fecha = r.GetDateTime(4),
                        Apuesta = r.GetDecimal(5),
                        Ganancia = r.GetDecimal(6),
                        Resultado = r.IsDBNull(7) ? null : r.GetString(7)
                    });

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

        // ── REGISTRO COMPLETO — ahora delega en PKG_PARTIDAS.pr_registrar_partida
        // Antes: ~50 líneas armando OracleTransaction manualmente en C#,
        //        con 2-3 INSERTs y commit/rollback explícitos.
        // Ahora: Oracle ejecuta todo en un solo bloque atómico.
        //        El trigger trg_actualizar_saldo sigue funcionando igual.

        public string RegistrarConMovimientos(Partida p)
        {
            using (OracleConnection con = ConexionOracle.Abrir())
            using (OracleCommand cmd = new OracleCommand("PKG_PARTIDAS.pr_registrar_partida", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.BindByName = true;

                cmd.Parameters.Add(new OracleParameter("p_id_usuario", p.IdUsuario));
                cmd.Parameters.Add(new OracleParameter("p_id_juego", p.IdJuego));
                cmd.Parameters.Add(new OracleParameter("p_id_estado", p.IdEstado));
                cmd.Parameters.Add(new OracleParameter("p_apuesta", p.Apuesta));
                cmd.Parameters.Add(new OracleParameter("p_ganancia", p.Ganancia));
                cmd.Parameters.Add(new OracleParameter("p_resultado", (object)p.Resultado ?? DBNull.Value));

                // Parámetro OUT: recibe 'Guardado correctamente.' o el error
                var pMsg = new OracleParameter("p_msg", OracleDbType.Varchar2, 200)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(pMsg);

                cmd.ExecuteNonQuery();

                return pMsg.Value.ToString();
            }
        }

        // ── Mapear — no cambia ────────────────────────────────────────────────

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