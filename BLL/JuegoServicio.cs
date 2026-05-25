using DAL;
using ENTITY;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class JuegoServicio
    {
        private readonly JuegoRepositorio _juegoRepo = new JuegoRepositorio();

        public IList<Juego> ObtenerTodos()
        {
            return _juegoRepo.Consultar();
        }

        public Juego ObtenerPorNombre(string nombre)
        {
            return _juegoRepo.ObtenerPorNombre(nombre);
        }

        public int ObtenerIdPorNombre(string nombre)
        {
            Juego juego = _juegoRepo.ObtenerPorNombre(nombre);
            return juego?.IdJuego ?? 0;
        }
    }
}
