using ENTITY;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.IO;

namespace DAL
{
    /// <summary>
    /// Agrega ObtenerPorUsuario() igual que TransaccionRepositorio.
    /// SiguienteIdPartida() de PartidaServicio desaparece:
    /// seq_partidas.NEXTVAL lo reemplaza en el INSERT.
    /// </summary>
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

        // Historial de partidas de un usuario específico.
        // Lo usa FrmCliente para mostrar el historial y
        // PartidaServicio para generar reportes por usuario.
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

        // El INSERT de partida NO maneja saldo aquí.
        // El saldo se mueve insertando en transacciones (que
        // dispara el trigger). La partida solo registra el
        // resultado histórico de la jugada.
        //
        // RETURNING id_partida INTO :nuevo_id permite recuperar
        // el ID generado por la secuencia justo después del INSERT,
        // lo que necesita PartidaServicio para asociar los detalles.
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
