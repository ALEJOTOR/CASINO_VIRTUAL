using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class PartidaServicio
    {
        private readonly PartidaRepositorio _partidaRepo = new PartidaRepositorio();
        private readonly EstadoPartidaRepositorio _estadoRepo = new EstadoPartidaRepositorio();
        private readonly UsuarioServicio _usuarioSvc = new UsuarioServicio();
        private readonly JuegoServicio _juegoSvc = new JuegoServicio();

        public string RegistrarPartida(Partida p)
        {
            if (p.Apuesta <= 0) return "La apuesta debe ser mayor a 0.";

            Usuario usuario = _usuarioSvc.ObtenerPorId(p.IdUsuario);
            if (usuario == null) return "Usuario no encontrado.";
            if (usuario.Saldo < p.Apuesta) return "Saldo insuficiente.";

            p.Fecha = DateTime.Now;
            return _partidaRepo.RegistrarConMovimientos(p);
        }

        public IList<Partida> ObtenerTodas()
        {
            return _partidaRepo.Consultar();
        }

        public IList<Partida> ObtenerPorUsuario(int idUsuario)
        {
            return _partidaRepo.ObtenerPorUsuario(idUsuario);
        }

        public int ObtenerIdJuegoPorNombre(string nombre)
        {
            return _juegoSvc.ObtenerIdPorNombre(nombre);
        }

        public IList<PartidaDisplayDto> ObtenerTodasConNombres()
        {
            IList<Partida> lista = _partidaRepo.Consultar();
            IList<Usuario> usuarios = _usuarioSvc.ObtenerTodos();
            IList<Juego> juegos = _juegoSvc.ObtenerTodos();
            Dictionary<int, string> mapaUsuarios = usuarios.ToDictionary(u => u.IdUsuario, u => $"{u.Nombre1} {u.Apellido1}");
            Dictionary<int, string> mapaJuegos = juegos.ToDictionary(j => j.IdJuego, j => j.Nombre);

            Dictionary<int, string> mapaEstados = _estadoRepo.Consultar()
                .ToDictionary(e => e.IdEstado, e => e.NombreEstado);

            return lista.Select(p => new PartidaDisplayDto
            {
                IdPartida = p.IdPartida,
                Usuario = mapaUsuarios.ContainsKey(p.IdUsuario) ? mapaUsuarios[p.IdUsuario] : $"Usuario {p.IdUsuario}",
                Juego = mapaJuegos.ContainsKey(p.IdJuego) ? mapaJuegos[p.IdJuego] : $"Juego {p.IdJuego}",
                Estado = mapaEstados.ContainsKey(p.IdEstado) ? mapaEstados[p.IdEstado] : $"Estado {p.IdEstado}",
                Fecha = p.Fecha,
                Apuesta = p.Apuesta,
                Ganancia = p.Ganancia
            }).ToList();
        }

        public string GenerarReporte()
        {
            IList<Partida> lista = _partidaRepo.Consultar();
            var adminIds = new HashSet<int>(
                _usuarioSvc.ObtenerTodos().Where(u => u.IdRol == 1).Select(u => u.IdUsuario)
            );
            lista = lista.Where(p => !adminIds.Contains(p.IdUsuario)).ToList();
            int total = lista.Count;
            int ganadas = lista.Count(p => p.IdEstado == 2);
            int perdidas = lista.Count(p => p.IdEstado == 3);
            decimal totalApostado = lista.Sum(p => p.Apuesta);
            decimal totalGanado = lista.Sum(p => p.Ganancia);

            return $"REPORTE DE PARTIDAS\n" +
                   $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}\n" +
                   $"─────────────────────────────\n" +
                   $"Total partidas:    {total}\n" +
                   $"Ganadas:           {ganadas}\n" +
                   $"Perdidas:          {perdidas}\n" +
                   $"Total apostado:    ${totalApostado:N2}\n" +
                   $"Total ganado:      ${totalGanado:N2}\n" +
                   $"Ganancia neta:     ${totalGanado - totalApostado:N2}\n";
        }

        public IList<HistorialPartida> ObtenerHistorialPorUsuario(int idUsuario)
        {
            return _partidaRepo.ObtenerHistorialPorUsuario(idUsuario);
        }

        public EstadisticasUsuario ObtenerEstadisticasUsuario(int idUsuario)
        {
            IList<Partida> partidas = _partidaRepo.ObtenerPorUsuario(idUsuario);

            int total = partidas.Count;
            int ganadas = partidas.Count(p => p.IdEstado == 2);
            int perdidas = partidas.Count(p => p.IdEstado == 3);
            decimal totalApostado = partidas.Sum(p => p.Apuesta);
            decimal totalGanado = partidas.Sum(p => p.Ganancia);

            string favorito = partidas
                .GroupBy(p => p.IdJuego)
                .OrderByDescending(g => g.Count())
                .Select(g =>
                {
                    Juego j = _juegoSvc.ObtenerTodos().FirstOrDefault(x => x.IdJuego == g.Key);
                    return j?.Nombre ?? "Desconocido";
                })
                .FirstOrDefault() ?? "N/A";

            int racha = 0;
            string tipoRacha = "ninguna";
            foreach (Partida p in partidas.OrderByDescending(p => p.Fecha))
            {
                if (p.IdEstado == 2 && (tipoRacha == "ninguna" || tipoRacha == "ganando"))
                {
                    racha++;
                    tipoRacha = "ganando";
                }
                else if (p.IdEstado == 3 && (tipoRacha == "ninguna" || tipoRacha == "perdiendo"))
                {
                    racha++;
                    tipoRacha = "perdiendo";
                }
                else break;
            }
            if (racha == 0) tipoRacha = "ninguna";

            return new EstadisticasUsuario
            {
                TotalPartidas = total,
                PartidasGanadas = ganadas,
                PartidasPerdidas = perdidas,
                TotalApostado = totalApostado,
                TotalGanado = totalGanado,
                GananciaNeta = totalGanado - totalApostado,
                JuegoFavorito = favorito,
                RachaActual = racha,
                TipoRacha = tipoRacha
            };
        }
    }
}
