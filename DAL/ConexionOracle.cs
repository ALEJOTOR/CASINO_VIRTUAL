using Oracle.ManagedDataAccess.Client;
using System;
using System.IO;
using System.Xml.Linq;

namespace DAL
{
    public static class ConexionOracle
    {
        private const string ArchivoConfigLocal = "conexion.local.config";

        private class ConfigOracle
        {
            public string Host { get; set; } = "localhost";
            public string Port { get; set; } = "1521";
            public string Service { get; set; } = "xepdb1";
            public string User { get; set; } = "sebas_casino";
            public string Password { get; set; } = "sebas11";
        }

        public static OracleConnection Abrir()
        {
            OracleConnection conexion = new OracleConnection(CrearCadenaConexion());
            conexion.Open();
            return conexion;
        }

        private static string CrearCadenaConexion()
        {
            ConfigOracle config = CargarConfiguracion();

            return $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)" +
                   $"(HOST={config.Host})(PORT={config.Port}))" +
                   $"(CONNECT_DATA=(SERVICE_NAME={config.Service})));" +
                   $"User Id={config.User};Password={config.Password};";
        }

        private static ConfigOracle CargarConfiguracion()
        {
            ConfigOracle config = new ConfigOracle();
            string ruta = BuscarConfigLocal();

            if (ruta == null)
                return config;

            XDocument doc = XDocument.Load(ruta);
            XElement root = doc.Element("conexionOracle");

            if (root == null)
                return config;

            config.Host = Leer(root, "host", config.Host);
            config.Port = Leer(root, "port", config.Port);
            config.Service = Leer(root, "service", config.Service);
            config.User = Leer(root, "user", config.User);
            config.Password = Leer(root, "password", config.Password);

            return config;
        }

        private static string BuscarConfigLocal()
        {
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            while (dir != null)
            {
                string ruta = Path.Combine(dir.FullName, ArchivoConfigLocal);
                if (File.Exists(ruta))
                    return ruta;

                dir = dir.Parent;
            }

            return null;
        }

        private static string Leer(XElement root, string nombre, string valorPorDefecto)
        {
            string valor = (string)root.Element(nombre);
            return string.IsNullOrWhiteSpace(valor) ? valorPorDefecto : valor.Trim();
        }
    }
}
