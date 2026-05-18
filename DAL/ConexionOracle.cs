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
        // ── Datos de conexión ────────────────────────────────────
        // Ajusta estos valores según tu instalación de Oracle:
        //   Host     → IP o nombre del servidor. En local es localhost.
        //   Port     → Puerto de Oracle. Por defecto siempre es 1521.
        //   Service  → El Service Name de tu BD. Suele ser "XE" en
        //              Oracle Express Edition (la versión gratuita)
        //              o "ORCL" en instalaciones estándar.
        //              Puedes verificarlo en SQL Developer en la
        //              configuración de tu conexión.
        //   User     → El esquema que creaste: casino_db
        //   Password → La contraseña que le diste al esquema
        // ────────────────────────────────────────────────────────
        private const string Host = "localhost";
        private const string Port = "1521";
        private const string Service = "xepdb1";        // <-- cambia si tu Service Name es diferente
        private const string User = "sebas_casino";
        private const string Password = "sebas11";

        // El formato Data Source es el estándar de Oracle.ManagedDataAccess.
        // No necesita tnsnames.ora ni Oracle Client instalado en la máquina.
        private static readonly string _cadena =
            $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)" +
            $"(HOST={Host})(PORT={Port}))" +
            $"(CONNECT_DATA=(SERVICE_NAME={Service})));" +
            $"User Id={User};Password={Password};";

        /// <summary>
        /// Devuelve una OracleConnection nueva y ya abierta.
        /// Cada repositorio la llama, la usa y la cierra.
        /// Usar using() en los repositorios garantiza que
        /// siempre se cierre aunque ocurra un error.
        /// </summary>
        public static OracleConnection Abrir()
        {
            OracleConnection conexion = new OracleConnection(_cadena);
            conexion.Open();
            return conexion;
        }
    }
}
