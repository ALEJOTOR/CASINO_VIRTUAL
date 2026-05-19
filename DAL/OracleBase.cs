using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;

namespace DAL
{
    public abstract class OracleBase<T>
    {
        public abstract IList<T> Consultar();
        public abstract string Guardar(T entidad);

        protected string EjecutarComando(string sql, (string nombre, object valor)[] parametros = null)
        {
            // using garantiza que la conexión se cierre siempre,
            // incluso si ocurre una excepción dentro del bloque.
            using (OracleConnection con = ConexionOracle.Abrir())
            using (OracleCommand cmd = new OracleCommand(sql, con))
            {
                if (parametros != null)
                    foreach (var (nombre, valor) in parametros)
                        cmd.Parameters.Add(new OracleParameter(nombre, valor));

                cmd.ExecuteNonQuery();
                return "Guardado correctamente.";
            }
        }
        protected OracleDataReader EjecutarConsulta(string sql, (string nombre, object valor)[] parametros = null)
        {
            OracleConnection con = ConexionOracle.Abrir();
            OracleCommand cmd = new OracleCommand(sql, con);

            if (parametros != null)
                foreach (var (nombre, valor) in parametros)
                    cmd.Parameters.Add(new OracleParameter(nombre, valor));
            return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }
    }
}