using DAL;
using ENTITY;
using System.Collections.Generic;

namespace BLL
{
    public class ApuestaServicio
    {
        private readonly ApuestaRepositorio _apuestaRepo = new ApuestaRepositorio();

        public string RegistrarApuesta(Apuesta a)
        {
            if (a.Monto <= 0)
                return "El monto de la apuesta debe ser mayor a 0.";
            if (a.IdPartida <= 0)
                return "La apuesta debe estar asociada a una partida valida.";

            return _apuestaRepo.Guardar(a);
        }

        public IList<Apuesta> ObtenerPorPartida(int idPartida)
        {
            return _apuestaRepo.ObtenerPorPartida(idPartida);
        }

        public IList<Apuesta> ObtenerTodas()
        {
            return _apuestaRepo.Consultar();
        }
    }
}
