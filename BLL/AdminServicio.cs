using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class AdminServicio
    {
        private readonly AdminRepositorio _adminRepo = new AdminRepositorio();

        public ResumenAdmin ObtenerResumenGeneral()
        {
            return _adminRepo.ObtenerResumenGeneral();
        }

        public IList<EstadisticasUsuario> ObtenerTopJugadores(int cantidad)
        {
            return _adminRepo.ObtenerTopJugadores(cantidad);
        }

        public Dictionary<string, int> ObtenerPartidasPorJuego()
        {
            return _adminRepo.ObtenerPartidasPorJuego();
        }

        public Dictionary<DateTime, decimal> ObtenerIngresosPorDia(int dias)
        {
            return _adminRepo.ObtenerIngresosPorDia(dias);
        }

        public Dictionary<DateTime, decimal> ObtenerGananciaCasaPorDia(int dias)
        {
            return _adminRepo.ObtenerGananciaCasaPorDia(dias);
        }

        public IList<Partida> ObtenerPartidasRecientes(int cantidad)
        {
            return _adminRepo.ObtenerPartidasRecientes(cantidad);
        }

        public IList<(int IdUsuario, decimal TotalDepositado, int NumDepositos)> ObtenerTopDepositantes(int cantidad)
        {
            return _adminRepo.ObtenerTopDepositantes(cantidad);
        }

        public decimal ObtenerTotalTransacciones(string tipo)
        {
            return _adminRepo.ObtenerTotalTransacciones(tipo);
        }

        public IList<(string Juego, int Partidas, decimal TotalApostado, decimal GananciaCasa, decimal Margen)> ObtenerRentabilidadPorJuego()
        {
            return _adminRepo.ObtenerRentabilidadPorJuego();
        }

        public Dictionary<string, (decimal Depositos, decimal Retiros)> ObtenerMovimientosPorMes(int meses)
        {
            return _adminRepo.ObtenerMovimientosPorMes(meses);
        }
    }
}
