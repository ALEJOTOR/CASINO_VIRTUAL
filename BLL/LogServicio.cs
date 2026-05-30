using DAL;
using ENTITY;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class LogServicio
    {
        private readonly LogRepositorio _logRepo = new LogRepositorio();

        public void Registrar(string tipoEvento, string nivel, string modulo, string descripcion, int? idUsuario = null, string ip = null)
        {
            try
            {
                _logRepo.Guardar(new LogEvento
                {
                    TipoEvento = tipoEvento,
                    Nivel = nivel,
                    Modulo = modulo,
                    Descripcion = descripcion,
                    IdUsuario = idUsuario,
                    IpOrigen = ip,
                    Fecha = DateTime.Now
                });
            }
            catch
            {
                // Swallow exception: el log nunca debe interrumpir el flujo principal
            }
        }

        public IList<LogEvento> ObtenerRecientes(int cantidad = 500)
        {
            return _logRepo.ObtenerRecientes(cantidad);
        }

        public IList<LogEvento> ObtenerFiltrados(string nivel, string tipo, DateTime desde, DateTime hasta)
        {
            return _logRepo.ObtenerFiltrados(nivel, tipo, desde, hasta);
        }

        public IList<LogEvento> ObtenerFiltradosDB(
            string nivel, string tipo,
            DateTime desde, DateTime hasta,
            int pagina, int porPagina,
            out int totalRegistros)
        {
            return _logRepo.ObtenerConFiltrosYPaginacion(
                nivel, tipo, desde, hasta, pagina, porPagina, out totalRegistros);
        }
    }
}
