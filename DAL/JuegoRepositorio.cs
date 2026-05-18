using ENTITY;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.IO;

namespace DAL
{
    /// <summary>
    /// Agrega ObtenerActivos() para que la GUI solo muestre
    /// juegos disponibles al cliente, sin traer los inactivos.
    /// Antes esto lo filtraba la BLL con un foreach; ahora
    /// el WHERE lo hace Oracle directamente.
    /// </summary>
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

        // Solo los juegos que el cliente puede jugar
        public IList<Juego> ObtenerActivos()
        {
            IList<Juego> lista = new List<Juego>();

            using (OracleDataReader reader = EjecutarConsulta(
                "SELECT id_juego, nombre, descripcion, estado FROM juegos WHERE estado = 'activo'"))
                while (reader.Read())
                    lista.Add(MapearJuego(reader));

            return lista;
        }

        public override string Guardar(Juego j)
        {
            return EjecutarComando(
                @"INSERT INTO juegos VALUES (seq_juegos.NEXTVAL, :nombre, :descripcion, :estado)",
                new[]
                {
                    (":nombre",      (object)j.Nombre),
                    (":descripcion", (object)(j.Descripcion ?? (object)System.DBNull.Value)),
                    (":estado",      (object)(j.Estado ?? "activo"))
                });
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
