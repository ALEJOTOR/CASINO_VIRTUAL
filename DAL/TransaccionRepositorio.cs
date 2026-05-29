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

        /// <summary>
        /// Obtiene transacciones paginadas de un usuario con filtros opcionales.
        /// </summary>
        /// <param name="idUsuario">ID del usuario</param>
        /// <param name="pageNumber">Número de página (1-based)</param>
        /// <param name="pageSize">Cantidad de registros por página</param>
        /// <param name="categoria">Filtro de tipo: null=todos, "deposito", "retiro"</param>
        /// <param name="fechaDesde">Filtro de fecha desde (inclusive)</param>
        /// <param name="fechaHasta">Filtro de fecha hasta (inclusive)</param>
        /// <returns>Tupla (lista de transacciones, total de registros que cumplen filtro)</returns>
        public (IList<Transaccion> items, int totalCount) ObtenerPaginado(
            int idUsuario, int pageNumber, int pageSize,
            string categoria = null, DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            // Parámetros para filtros (usados en COUNT y PAGED)
            var filterParameters = new List<(string, object)>
            {
                (":idUsuario", (object)idUsuario)
            };

            // Parámetros de paginación (solo en PAGED)
            var allParameters = new List<(string, object)>
            {
                (":idUsuario", (object)idUsuario),
                (":offset", (object)((pageNumber - 1) * pageSize)),
                (":pageSize", (object)pageSize)
            };

            // Construcción dinámica del WHERE
            string whereClause = "WHERE id_usuario = :idUsuario";
            
            if (!string.IsNullOrEmpty(categoria))
            {
                if (categoria.ToLower() == "deposito" || categoria.ToLower() == "depositos")
                {
                    whereClause += " AND tipo IN ('deposito', 'retiro')";
                }
                else if (categoria.ToLower() == "apuesta" || categoria.ToLower() == "apuestas")
                {
                    whereClause += " AND tipo IN ('ganancia', 'perdida')";
                }
            }

            if (fechaDesde.HasValue)
            {
                whereClause += " AND fecha >= :fechaDesde";
                filterParameters.Add((":fechaDesde", (object)fechaDesde.Value));
                allParameters.Add((":fechaDesde", (object)fechaDesde.Value));
            }

            if (fechaHasta.HasValue)
            {
                whereClause += " AND fecha <= :fechaHasta";
                filterParameters.Add((":fechaHasta", (object)fechaHasta.Value));
                allParameters.Add((":fechaHasta", (object)fechaHasta.Value));
            }

            // Query para obtener el total (solo parámetros de filtro)
            string sqlCount = string.Format("SELECT COUNT(*) FROM transacciones {0}", whereClause);
            int totalCount = 0;
            using (OracleDataReader reader = EjecutarConsulta(sqlCount, filterParameters.ToArray()))
            {
                if (reader.Read())
                    totalCount = reader.GetInt32(0);
            }

            // Query para obtener la página (Oracle: OFFSET/FETCH con todos los parámetros)
            string sqlPaged = string.Format(@"SELECT id_transaccion, id_usuario, tipo, monto, fecha, descripcion
                                FROM transacciones
                                {0}
                                ORDER BY fecha DESC
                                OFFSET :offset ROWS
                                FETCH NEXT :pageSize ROWS ONLY", whereClause);

            IList<Transaccion> items = new List<Transaccion>();
            using (OracleDataReader reader = EjecutarConsulta(sqlPaged, allParameters.ToArray()))
            {
                while (reader.Read())
                    items.Add(Mapear(reader));
            }

            return (items, totalCount);
        }

        public override string Guardar(Transaccion t)
        {
            EjecutarComando(@"INSERT INTO transacciones (
                                id_transaccion, id_usuario, tipo,
                                monto, fecha, descripcion
                            ) VALUES (
                                seq_transacciones.NEXTVAL, :id_usuario, :tipo,
                                :monto, CURRENT_TIMESTAMP, :descripcion
                            )", new[]
            {
                (":id_usuario",  (object)t.IdUsuario),
                (":tipo",        (object)t.Tipo),
                (":monto",       (object)t.Monto),
                (":descripcion", (object)(t.Descripcion ?? (object)DBNull.Value))
            });
            return "Guardado correctamente.";
        }

        public string RegistrarDepositoConSaldo(int idUsuario, decimal monto)
        {
            return EjecutarSP("PKG_USUARIOS.pr_realizar_deposito", "p_resultado",
                ("p_id_usuario", idUsuario),
                ("p_monto", monto));
        }

        private Transaccion Mapear(OracleDataReader r)
        {
            return new Transaccion
            {
                IdTransaccion = r.GetInt32(0),
                IdUsuario = r.GetInt32(1),
                Tipo = r.GetString(2),
                Monto = r.GetDecimal(3),
                Fecha = r.GetDateTime(4),
                Descripcion = r.IsDBNull(5) ? null : r.GetString(5)
            };
        }
    }
}