using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class AdminServicio
    {
        private readonly UsuarioRepositorio _usuarioRepo = new UsuarioRepositorio();
        private readonly PartidaRepositorio _partidaRepo = new PartidaRepositorio();
        private readonly TransaccionRepositorio _transRepo = new TransaccionRepositorio();
        private readonly JuegoRepositorio _juegoRepo = new JuegoRepositorio();

        public ResumenAdmin ObtenerResumenGeneral()
        {
            IList<Partida> partidas = _partidaRepo.Consultar();
            IList<Usuario> usuarios = _usuarioRepo.Consultar();
            IList<Transaccion> transacciones = _transRepo.Consultar();
            IList<Juego> juegos = _juegoRepo.Consultar();

            DateTime hoy = DateTime.Today;
            DateTime manana = hoy.AddDays(1);

            IList<Partida> partidasHoy = partidas.Where(p => p.Fecha >= hoy && p.Fecha < manana).ToList();

            decimal totalApostadoHoy = partidasHoy.Sum(p => p.Apuesta);
            decimal gananciaCasaHoy = partidasHoy.Sum(p => p.Apuesta - p.Ganancia);
            decimal gananciaCasaTotal = partidas.Sum(p => p.Apuesta - p.Ganancia);

            var grupoJuego = partidas.GroupBy(p => p.IdJuego)
                .OrderByDescending(g => g.Count()).FirstOrDefault();
            Dictionary<int, string> mapaJuego = juegos.ToDictionary(j => j.IdJuego, j => j.Nombre);
            string juegoTop = grupoJuego != null && mapaJuego.ContainsKey(grupoJuego.Key)
                ? mapaJuego[grupoJuego.Key] : "N/A";

            var grupoUser = partidas.GroupBy(p => p.IdUsuario)
                .OrderByDescending(g => g.Count()).FirstOrDefault();
            Dictionary<int, string> mapaUser = usuarios.ToDictionary(u => u.IdUsuario,
                u => $"{u.Nombre1} {u.Apellido1}");
            string userTop = grupoUser != null && mapaUser.ContainsKey(grupoUser.Key)
                ? mapaUser[grupoUser.Key] : "N/A";

            return new ResumenAdmin
            {
                TotalUsuarios = usuarios.Count,
                UsuariosActivos = usuarios.Count(u => u.Estado == "activo"),
                UsuariosActivosHoy = usuarios.Count(u => u.Estado == "activo"),
                IngresosHoy = transacciones
                    .Where(t => t.Fecha >= hoy && t.Fecha < manana && t.Tipo == "deposito")
                    .Sum(t => t.Monto),
                IngresosTotal = transacciones.Where(t => t.Tipo == "deposito").Sum(t => t.Monto),
                PartidasHoy = partidasHoy.Count,
                PartidasTotal = partidas.Count,
                TotalApostadoHoy = totalApostadoHoy,
                GananciaCasaHoy = gananciaCasaHoy,
                GananciaCasaTotal = gananciaCasaTotal,
                PromedioApuesta = partidas.Count > 0 ? partidas.Average(p => p.Apuesta) : 0m,
                PromedioApuestaHoy = partidasHoy.Count > 0 ? partidasHoy.Average(p => p.Apuesta) : 0m,
                JuegoMasJugado = juegoTop,
                PartidasJuegoMasJugado = grupoJuego?.Count() ?? 0,
                UsuarioMasActivo = userTop,
                PartidasUsuarioMasActivo = grupoUser?.Count() ?? 0
            };
        }

        public IList<EstadisticasUsuario> ObtenerTopJugadores(int cantidad)
        {
            IList<Partida> todas = _partidaRepo.Consultar();
            IList<Usuario> usuarios = _usuarioRepo.Consultar();
            Dictionary<int, Usuario> mapaUsuarios = usuarios.ToDictionary(u => u.IdUsuario);

            return todas
                .GroupBy(p => p.IdUsuario)
                .Select(g => new
                {
                    IdUsuario = g.Key,
                    TotalApostado = g.Sum(p => p.Apuesta),
                    TotalGanado = g.Sum(p => p.Ganancia),
                    TotalPartidas = g.Count(),
                    Ganadas = g.Count(p => p.Resultado == "gano"),
                    Perdidas = g.Count(p => p.Resultado == "perdio")
                })
                .OrderByDescending(x => x.TotalApostado)
                .Take(cantidad)
                .Select(t =>
                {
                    Usuario u = mapaUsuarios.ContainsKey(t.IdUsuario) ? mapaUsuarios[t.IdUsuario] : null;
                    return new EstadisticasUsuario
                    {
                        IdUsuario = t.IdUsuario,
                        NombreUsuario = u != null ? $"{u.Nombre1} {u.Apellido1}" : "Desconocido",
                        Username = u?.Username ?? "N/A",
                        Saldo = u?.Saldo ?? 0m,
                        TotalPartidas = t.TotalPartidas,
                        PartidasGanadas = t.Ganadas,
                        PartidasPerdidas = t.Perdidas,
                        TotalApostado = t.TotalApostado,
                        TotalGanado = t.TotalGanado,
                        GananciaNeta = t.TotalGanado - t.TotalApostado
                    };
                })
                .ToList();
        }

        public Dictionary<string, int> ObtenerPartidasPorJuego()
        {
            IList<Partida> todas = _partidaRepo.Consultar();
            IList<Juego> juegos = _juegoRepo.Consultar();
            Dictionary<int, string> nombreJuego = juegos.ToDictionary(j => j.IdJuego, j => j.Nombre);

            return todas
                .GroupBy(p => p.IdJuego)
                .ToDictionary(
                    g => nombreJuego.ContainsKey(g.Key) ? nombreJuego[g.Key] : $"Juego {g.Key}",
                    g => g.Count());
        }

        public Dictionary<DateTime, decimal> ObtenerIngresosPorDia(int dias)
        {
            IList<Transaccion> transacciones = _transRepo.Consultar();
            DateTime limite = DateTime.Today.AddDays(-dias + 1);

            var depositos = transacciones
                .Where(t => t.Tipo == "deposito" && t.Fecha >= limite)
                .GroupBy(t => t.Fecha.Date)
                .ToDictionary(g => g.Key, g => g.Sum(t => t.Monto));

            Dictionary<DateTime, decimal> resultado = new Dictionary<DateTime, decimal>();
            for (int i = dias - 1; i >= 0; i--)
            {
                DateTime fecha = DateTime.Today.AddDays(-i);
                resultado[fecha] = depositos.ContainsKey(fecha) ? depositos[fecha] : 0m;
            }
            return resultado;
        }

        public Dictionary<DateTime, decimal> ObtenerGananciaCasaPorDia(int dias)
        {
            IList<Partida> partidas = _partidaRepo.Consultar();
            DateTime limite = DateTime.Today.AddDays(-dias + 1);

            var agrupadas = partidas
                .Where(p => p.Fecha >= limite)
                .GroupBy(p => p.Fecha.Date)
                .ToDictionary(g => g.Key, g => g.Sum(p => p.Apuesta - p.Ganancia));

            Dictionary<DateTime, decimal> resultado = new Dictionary<DateTime, decimal>();
            for (int i = dias - 1; i >= 0; i--)
            {
                DateTime fecha = DateTime.Today.AddDays(-i);
                resultado[fecha] = agrupadas.ContainsKey(fecha) ? agrupadas[fecha] : 0m;
            }
            return resultado;
        }

        public IList<Partida> ObtenerPartidasRecientes(int cantidad)
        {
            return _partidaRepo.Consultar().Take(cantidad).ToList();
        }
    }
}
