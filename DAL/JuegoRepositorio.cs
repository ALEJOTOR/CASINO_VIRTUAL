using ENTITY;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;

namespace DAL
{
    public class JuegoRepositorio : OracleBase<Juego>
    {
        public override IList<Juego> Consultar()
        {
            IList<Juego> lista = new List<Juego>();

            using (OracleDataReader reader = EjecutarConsulta(
                "SELECT id_juego, nombre, descripcion, estado FROM juegos ORDER BY id_juego"))
                while (reader.Read())
                    lista.Add(MapearJuego(reader));

            return lista;
        }

        public IList<Juego> ObtenerActivos()
        {
            IList<Juego> lista = new List<Juego>();

            using (OracleDataReader reader = EjecutarConsulta(
                "SELECT id_juego, nombre, descripcion, estado FROM juegos WHERE estado = 'activo'"))
                while (reader.Read())
                    lista.Add(MapearJuego(reader));

            return lista;
        }

        public Juego ObtenerPorNombre(string nombre)
        {
            using (OracleDataReader reader = EjecutarConsulta(
                "SELECT id_juego, nombre, descripcion, estado FROM juegos WHERE nombre = :nombre",
                new[] { (":nombre", (object)nombre) }))
                if (reader.Read())
                    return MapearJuego(reader);

            return null;
        }

        public override string Guardar(Juego j)
        {
            EjecutarComando(
                @"INSERT INTO juegos VALUES (seq_juegos.NEXTVAL, :nombre, :descripcion, :estado)",
                new[]
                {
                    (":nombre",      (object)j.Nombre),
                    (":descripcion", (object)(j.Descripcion ?? (object)System.DBNull.Value)),
                    (":estado",      (object)(j.Estado ?? "activo"))
                });
            return "Guardado correctamente.";
        }

        private Juego MapearJuego(OracleDataReader r)
        {
            return new Juego
            {
                IdJuego = r.GetInt32(0),
                Nombre = r.GetString(1),
                Descripcion = r.IsDBNull(2) ? null : r.GetString(2),
                Estado = r.GetString(3)
            };
        }
    }
}
