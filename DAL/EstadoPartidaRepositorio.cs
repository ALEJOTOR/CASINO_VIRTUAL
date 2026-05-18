using ENTITY;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;

namespace DAL
{
    /// <summary>
    /// Repositorio de estados de partida en Oracle.
    /// </summary>
    public class EstadoPartidaRepositorio : OracleBase<EstadoPartida>
    {
        public override IList<EstadoPartida> Consultar()
        {
            IList<EstadoPartida> lista = new List<EstadoPartida>();

            using (OracleDataReader reader = EjecutarConsulta(
                "SELECT id_estado, nombre_estado, descripcion FROM estado_partidas ORDER BY id_estado"))
                while (reader.Read())
                    lista.Add(Mapear(reader));

            return lista;
        }

        public override string Guardar(EstadoPartida estado)
        {
            return EjecutarComando(
                @"INSERT INTO estado_partidas (id_estado, nombre_estado, descripcion)
                  VALUES (:id_estado, :nombre_estado, :descripcion)",
                new[]
                {
                    (":id_estado", (object)estado.IdEstado),
                    (":nombre_estado", (object)estado.NombreEstado),
                    (":descripcion", (object)(estado.Descripcion ?? (object)System.DBNull.Value))
                });
        }

        private EstadoPartida Mapear(OracleDataReader r)
        {
            return new EstadoPartida
            {
                IdEstado = r.GetInt32(0),
                NombreEstado = r.GetString(1),
                Descripcion = r.IsDBNull(2) ? null : r.GetString(2)
            };
        }
    }
}
