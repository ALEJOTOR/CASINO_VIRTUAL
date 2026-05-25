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
        private readonly TransaccionRepositorio _transRepo = new TransaccionRepositorio();
        private readonly JuegoRepositorio _juegoRepo = new JuegoRepositorio();
        private readonly UsuarioServicio _usuarioSvc = new UsuarioServicio();

        // ── Consultas ─────────────────────────────────────────────

        public IList<Partida> ObtenerTodas()
        {
            return _partidaRepo.Consultar();
        }

        public IList<Partida> ObtenerPorUsuario(int idUsuario)
        {
            return _partidaRepo.ObtenerPorUsuario(idUsuario);
        }

        public IList<HistorialPartida> ObtenerHistorialPorUsuario(int idUsuario)
        {
            return _partidaRepo.ObtenerHistorialPorUsuario(idUsuario);
        }

        public IList<Transaccion> ObtenerTransaccionesPorUsuario(int idUsuario)
        {
            return _transRepo.ObtenerPorUsuario(idUsuario);
        }

        public IList<Transaccion> ObtenerTodasTransacciones()
        {
            return _transRepo.Consultar();
        }

        public IList<Juego> ObtenerJuegos()
        {
            return _juegoRepo.Consultar();
        }

        public int ObtenerIdJuegoPorNombre(string nombre)
        {
            Juego juego = _juegoRepo.ObtenerPorNombre(nombre);
            return juego == null ? 0 : juego.IdJuego;
        }

        // ── Operaciones ───────────────────────────────────────────

        public string RegistrarPartida(Partida p)
        {
            if (p.Apuesta <= 0) return "La apuesta debe ser mayor a 0.";

            Usuario usuario = _usuarioSvc.ObtenerPorId(p.IdUsuario);
            if (usuario == null) return "Usuario no encontrado.";
            if (usuario.Saldo < p.Apuesta) return "Saldo insuficiente.";

            p.Fecha = DateTime.Now;
            return _partidaRepo.RegistrarConMovimientos(p);
        }

        public string RealizarDeposito(int idUsuario, decimal monto)
        {
            if (monto <= 0) return "El monto debe ser mayor a 0.";

            return _transRepo.RegistrarDepositoConSaldo(idUsuario, monto);
        }

        public string RealizarRetiro(int idUsuario, decimal monto)
        {
            return _usuarioSvc.RetirarSaldo(idUsuario, monto);
        }

        // ── Reportes ──────────────────────────────────────────────

        public string GenerarReporte()
        {
            IList<Partida> partidas = _partidaRepo.Consultar();
            int ganadas = partidas.Count(p => p.Resultado == "gano");
            int perdidas = partidas.Count(p => p.Resultado == "perdio");
            decimal apostado = partidas.Sum(p => p.Apuesta);
            decimal ganado = partidas.Sum(p => p.Ganancia);

            return $"REPORTE DE PARTIDAS\n" +
                   $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}\n" +
                   $"─────────────────────────────\n" +
                   $"Total partidas:    {partidas.Count}\n" +
                   $"Ganadas:           {ganadas}\n" +
                   $"Perdidas:          {perdidas}\n" +
                   $"Total apostado:    ${apostado:N2}\n" +
                   $"Total ganado:      ${ganado:N2}\n" +
                   $"Balance casino:    ${apostado - ganado:N2}\n";
        }

    }
}
