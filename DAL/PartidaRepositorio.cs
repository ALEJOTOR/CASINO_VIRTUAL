using ENTITY;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

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

                var pMsg = new OracleParameter("p_msg", OracleDbType.Varchar2, 200)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(pMsg);

                cmd.ExecuteNonQuery();

                return pMsg.Value.ToString();
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