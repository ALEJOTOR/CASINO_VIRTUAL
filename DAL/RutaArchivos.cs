using System.IO;

namespace DAL
{
    /// <summary>
    /// Centraliza la ruta de los archivos de datos.
    /// Se crean en la misma carpeta del ejecutable.
    /// </summary>
    public static class RutaArchivos
    {
        private static readonly string _base = Directory.GetCurrentDirectory() + @"\Datos\";

        public static string Roles        => _base + "roles.txt";
        public static string Usuarios     => _base + "usuarios.txt";
        public static string Juegos       => _base + "juegos.txt";
        public static string Partidas     => _base + "partidas.txt";
        public static string Transacciones => _base + "transacciones.txt";
        public static string EstadoPartidas => _base + "estado_partidas.txt";

        public static void CrearCarpeta()
        {
            if (!Directory.Exists(_base))
                Directory.CreateDirectory(_base);
        }
    }
}
