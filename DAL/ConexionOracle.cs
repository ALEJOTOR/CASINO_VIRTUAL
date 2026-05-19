using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class ConexionOracle
    {
        private const string Host = "localhost";
        private const string Port = "1521";
        private const string Service = "xepdb1";       
        private const string User = "sebas_casino";
        private const string Password = "sebas11";

        private static readonly string _cadena =
            $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)" +
            $"(HOST={Host})(PORT={Port}))" +
            $"(CONNECT_DATA=(SERVICE_NAME={Service})));" +
            $"User Id={User};Password={Password};";

        public static OracleConnection Abrir()
        {
            OracleConnection conexion = new OracleConnection(_cadena);
            conexion.Open();
            return conexion;
        }
    }
}
