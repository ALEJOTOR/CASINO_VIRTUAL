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
        private readonly UsuarioServicio _usuarioSvc = new UsuarioServicio();

        // ── Consultas ─────────────────────────────────────────────

        public IList<Partida> ObtenerTodas()
        {
            return _partidaRepo.Consultar();
        }

        public IList<Partida> ObtenerPorUsuario(int idUsuario)
        {
            IList<Partida> resultado = new List<Partida>();
            foreach (Partida p in _partidaRepo.Consultar())
                if (p.IdUsuario == idUsuario) resultado.Add(p);
            return resultado;
        }

        public IList<Transaccion> ObtenerTransaccionesPorUsuario(int idUsuario)
        {
            IList<Transaccion> resultado = new List<Transaccion>();
            foreach (Transaccion t in _transRepo.Consultar())
                if (t.IdUsuario == idUsuario) resultado.Add(t);
            return resultado;
        }

        public IList<Transaccion> ObtenerTodasTransacciones()
        {
            return _transRepo.Consultar();
        }

        // ── Operaciones ───────────────────────────────────────────

        public string RegistrarPartida(Partida p)
        {
            if (p.Apuesta <= 0) return "La apuesta debe ser mayor a 0.";

            Usuario usuario = _usuarioSvc.ObtenerPorId(p.IdUsuario);
            if (usuario == null) return "Usuario no encontrado.";
            if (usuario.Saldo < p.Apuesta) return "Saldo insuficiente.";

            // Descontar apuesta
            _usuarioSvc.ActualizarSaldo(p.IdUsuario, -p.Apuesta);
            RegistrarTransaccion(p.IdUsuario, "pérdida", p.Apuesta,
                $"Apuesta partida juego {p.IdJuego}");

            // Acreditar ganancia si aplica
            if (p.Resultado == "gano" && p.Ganancia > 0)
            {
                _usuarioSvc.ActualizarSaldo(p.IdUsuario, p.Ganancia);
                RegistrarTransaccion(p.IdUsuario, "ganancia", p.Ganancia,
                    $"Ganancia partida juego {p.IdJuego}");
            }

            p.IdPartida = SiguienteIdPartida();
            p.Fecha = DateTime.Now;
            return _partidaRepo.Guardar(p);
        }

        public string RealizarDeposito(int idUsuario, decimal monto)
        {
            if (monto <= 0) return "El monto debe ser mayor a 0.";

            string resultado = _usuarioSvc.ActualizarSaldo(idUsuario, monto);
            if (resultado != "Guardado correctamente.") return resultado;

            RegistrarTransaccion(idUsuario, "depósito", monto, "Recarga de saldo");
            return "Depósito realizado correctamente.";
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

        // ── Helpers privados ──────────────────────────────────────

        private void RegistrarTransaccion(int idUsuario, string tipo, decimal monto, string descripcion)
        {
            Transaccion t = new Transaccion
            {
                IdTransaccion = SiguienteIdTransaccion(),
                IdUsuario = idUsuario,
                Tipo = tipo,
                Monto = monto,
                Fecha = DateTime.Now,
                Descripcion = descripcion
            };
            _transRepo.Guardar(t);
        }

        private int SiguienteIdPartida()
        {
            IList<Partida> lista = _partidaRepo.Consultar();
            if (lista.Count == 0) return 1;
            int max = 0;
            foreach (Partida p in lista) if (p.IdPartida > max) max = p.IdPartida;
            return max + 1;
        }

        private int SiguienteIdTransaccion()
        {
            IList<Transaccion> lista = _transRepo.Consultar();
            if (lista.Count == 0) return 1;
            int max = 0;
            foreach (Transaccion t in lista) if (t.IdTransaccion > max) max = t.IdTransaccion;
            return max + 1;
        }
    }
}
