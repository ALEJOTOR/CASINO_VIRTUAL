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
            EjecutarComando(@"INSERT INTO partidas (
                                id_partida, id_usuario, id_juego,
                                id_estado, fecha, apuesta,
                                ganancia, resultado
                            ) VALUES (
                                seq_partidas.NEXTVAL, :id_usuario, :id_juego,
                                :id_estado, CURRENT_TIMESTAMP, :apuesta,
                                :ganancia, :resultado
                            )", new[]
            {
                (":id_usuario", (object)p.IdUsuario),
                (":id_juego",   (object)p.IdJuego),
                (":id_estado",  (object)p.IdEstado),
                (":apuesta",    (object)p.Apuesta),
                (":ganancia",   (object)p.Ganancia),
                (":resultado",  (object)(p.Resultado ?? (object)DBNull.Value))
            });
            return "Guardado correctamente.";
        }

        public string RegistrarConMovimientos(Partida p)
        {
            return EjecutarSP("PKG_PARTIDAS.pr_registrar_partida", "p_msg",
                ("p_id_usuario", p.IdUsuario),
                ("p_id_juego", p.IdJuego),
                ("p_id_estado", p.IdEstado),
                ("p_apuesta", p.Apuesta),
                ("p_ganancia", p.Ganancia),
                ("p_resultado", (object)p.Resultado ?? DBNull.Value));
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