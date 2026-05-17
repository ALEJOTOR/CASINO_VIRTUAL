using System.IO;

namespace DAL
{
    public static class RutaArchivos
    {
        private static readonly string _base =
            Path.Combine(Directory.GetCurrentDirectory(), "Datos");

        public static string Roles          => Path.Combine(_base, "roles.txt");
        public static string Usuarios       => Path.Combine(_base, "usuarios.txt");
        public static string Juegos         => Path.Combine(_base, "juegos.txt");
        public static string Partidas       => Path.Combine(_base, "partidas.txt");
        public static string Transacciones  => Path.Combine(_base, "transacciones.txt");
        public static string EstadoPartidas => Path.Combine(_base, "estado_partidas.txt");
    }
}
