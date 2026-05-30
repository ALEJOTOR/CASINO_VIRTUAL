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
                                  ganancia
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
                                  ganancia
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
                                  ganancia
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
                        Ganancia = r.GetDecimal(6)
                    });

            return lista;
        }

        public override string Guardar(Partida p)
        {
            EjecutarComando(@"INSERT INTO partidas (
                                id_partida, id_usuario, id_juego,
                                id_estado, fecha, apuesta,
                                ganancia
                            ) VALUES (
                                seq_partidas.NEXTVAL, :id_usuario, :id_juego,
                                :id_estado, CURRENT_TIMESTAMP, :apuesta,
                                :ganancia
                            )", new[]
            {
                (":id_usuario", (object)p.IdUsuario),
                (":id_juego",   (object)p.IdJuego),
                (":id_estado",  (object)p.IdEstado),
                (":apuesta",    (object)p.Apuesta),
                (":ganancia",   (object)p.Ganancia)
            });
            return "Guardado correctamente.";
        }

        public (string mensaje, int idPartida) RegistrarConMovimientos(Partida p)
        {
            using (OracleConnection con = ConexionOracle.Abrir())
            using (OracleCommand cmd = new OracleCommand("PKG_PARTIDAS.pr_registrar_partida", con))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.BindByName = true;

                cmd.Parameters.Add(new OracleParameter("p_id_usuario", OracleDbType.Int32)
                    { Value = p.IdUsuario, Direction = System.Data.ParameterDirection.Input });
                cmd.Parameters.Add(new OracleParameter("p_id_juego", OracleDbType.Int32)
                    { Value = p.IdJuego, Direction = System.Data.ParameterDirection.Input });
                cmd.Parameters.Add(new OracleParameter("p_id_estado", OracleDbType.Int32)
                    { Value = p.IdEstado, Direction = System.Data.ParameterDirection.Input });
                cmd.Parameters.Add(new OracleParameter("p_apuesta", OracleDbType.Decimal)
                    { Value = p.Apuesta, Direction = System.Data.ParameterDirection.Input });
                cmd.Parameters.Add(new OracleParameter("p_ganancia", OracleDbType.Decimal)
                    { Value = p.Ganancia, Direction = System.Data.ParameterDirection.Input });
                cmd.Parameters.Add(new OracleParameter("p_id_partida", OracleDbType.Int32)
                    { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new OracleParameter("p_msg", OracleDbType.Varchar2, 200)
                    { Direction = System.Data.ParameterDirection.Output });

                cmd.ExecuteNonQuery();

                int idPartida = Convert.ToInt32(cmd.Parameters["p_id_partida"].Value?.ToString() ?? "0");
                string mensaje = cmd.Parameters["p_msg"].Value?.ToString() ?? string.Empty;

                return (mensaje, idPartida);
            }
        }

        public decimal ObtenerMultiplicadorBono(int idUsuario)
        {
            string sql = "SELECT FN_CALCULAR_GANANCIA_CON_BONO(:id, 1) FROM DUAL";
            try
            {
                return EjecutarScalar<decimal>(sql, new[] { (":id", (object)idUsuario) });
            }
            catch
            {
                return 1m;
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
                Ganancia = r.GetDecimal(6)
            };
        }
    }
}