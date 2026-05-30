using DAL;
using ENTITY;

namespace BLL
{
    public class TelegramVinculoServicio
    {
        private readonly TelegramVinculoRepositorio _repo = new TelegramVinculoRepositorio();

        public TelegramVinculo ObtenerPendientePorUsuario(int idUsuario)
        {
            return _repo.ObtenerPendientePorUsuario(idUsuario);
        }

        public string ConfirmarVinculo(int idUsuario, string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                return "No hay codigo pendiente para confirmar.";

            TelegramVinculo pendiente = _repo.ObtenerPendientePorUsuario(idUsuario);
            if (pendiente == null)
                return "No hay solicitudes pendientes o el codigo expiro.";

            if (pendiente.Codigo != codigo.Trim())
                return "El codigo no coincide.";

            return _repo.ConfirmarVinculo(idUsuario, codigo.Trim());
        }

        public bool ExisteVinculoActivo(int idUsuario)
        {
            return _repo.ExisteVinculoActivo(idUsuario);
        }
    }
}
