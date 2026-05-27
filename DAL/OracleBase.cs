using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace DAL
{
    public abstract class OracleBase<T>
    {
        public abstract IList<T> Consultar();
        public abstract string Guardar(T entidad);

        protected void EjecutarComando(string sql, (string nombre, object valor)[] parametros = null)
        {
            using (OracleConnection con = ConexionOracle.Abrir())
            using (OracleCommand cmd = new OracleCommand(sql, con))
            {
                if (parametros != null)
                    foreach (var (nombre, valor) in parametros)
                        cmd.Parameters.Add(new OracleParameter(nombre, valor ?? DBNull.Value));

                cmd.ExecuteNonQuery();
            }
        }

        protected OracleDataReader EjecutarConsulta(string sql, (string nombre, object valor)[] parametros = null)
        {
            OracleConnection con = ConexionOracle.Abrir();
            OracleCommand cmd = new OracleCommand(sql, con);

            if (parametros != null)
                foreach (var (nombre, valor) in parametros)
                    cmd.Parameters.Add(new OracleParameter(nombre, valor ?? DBNull.Value));

            return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }

        protected string EjecutarSP(string nombreProcedimiento,
            string nombreParametroSalida,
            params (string nombre, object valor)[] parametrosEntrada)
        {
            using (OracleConnection con = ConexionOracle.Abrir())
            using (OracleCommand cmd = new OracleCommand(nombreProcedimiento, con))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.BindByName = true;

                foreach (var (nombre, valor) in parametrosEntrada)
                    cmd.Parameters.Add(new OracleParameter(nombre, valor ?? DBNull.Value));

                var pOut = new OracleParameter(nombreParametroSalida, OracleDbType.Varchar2, 200)
                {
                    Direction = System.Data.ParameterDirection.Output
                };
                cmd.Parameters.Add(pOut);

                cmd.ExecuteNonQuery();
                return pOut.Value?.ToString() ?? string.Empty;
            }
        }
    }
}