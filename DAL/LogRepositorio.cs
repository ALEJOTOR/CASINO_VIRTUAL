using ENTITY;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class LogRepositorio : OracleBase<LogEvento>
    {
        public override IList<LogEvento> Consultar()
        {
            return ObtenerRecientes(500);
        }

        public IList<LogEvento> ObtenerRecientes(int cantidad)
        {
            IList<LogEvento> lista = new List<LogEvento>();

            string sql = @"SELECT id_log, tipo_evento, nivel, id_usuario, username,
                                  ip_origen, modulo, descripcion, fecha
                             FROM fn_obtener_log_reciente(:p_cantidad)";

            using (OracleDataReader r = EjecutarConsulta(sql, new[] { (":p_cantidad", (object)cantidad) }))
                while (r.Read())
                    lista.Add(Mapear(r));

            return lista;
        }

        public IList<LogEvento> ObtenerFiltrados(string nivel, string tipo, DateTime desde, DateTime hasta)
        {
            IList<LogEvento> todos = ObtenerRecientes(1000);
            var filtrados = new List<LogEvento>();

            foreach (LogEvento log in todos)
            {
                if (!string.IsNullOrEmpty(nivel) && nivel != "Todos" && log.Nivel != nivel)
                    continue;
                if (!string.IsNullOrEmpty(tipo) && tipo != "Todos" && log.TipoEvento != tipo)
                    continue;
                if (log.Fecha < desde || log.Fecha > hasta)
                    continue;
                filtrados.Add(log);
            }

            return filtrados;
        }

        public override string Guardar(LogEvento log)
        {
            EjecutarComando(
                "BEGIN pr_registrar_log(:p_tipo_evento, :p_nivel, :p_id_usuario, :p_ip_origen, :p_modulo, :p_descripcion); END;",
                new[]
                {
                    (":p_tipo_evento", (object)log.TipoEvento),
                    (":p_nivel", (object)log.Nivel),
                    (":p_id_usuario", (object)log.IdUsuario ?? DBNull.Value),
                    (":p_ip_origen", (object)log.IpOrigen ?? DBNull.Value),
                    (":p_modulo", (object)log.Modulo),
                    (":p_descripcion", (object)log.Descripcion)
                });
            return "Guardado correctamente.";
        }

        private LogEvento Mapear(OracleDataReader r)
        {
            return new LogEvento
            {
                IdLog = r.GetInt32(0),
                TipoEvento = r.GetString(1),
                Nivel = r.GetString(2),
                IdUsuario = r.IsDBNull(3) ? (int?)null : r.GetInt32(3),
                Username = r.IsDBNull(4) ? null : r.GetString(4),
                IpOrigen = r.IsDBNull(5) ? null : r.GetString(5),
                Modulo = r.GetString(6),
                Descripcion = r.GetString(7),
                Fecha = r.GetDateTime(8)
            };
        }
    }
}
