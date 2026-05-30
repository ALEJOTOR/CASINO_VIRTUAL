using System;

namespace ENTITY
{
    public class Bono
    {
        public int IdBono { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int? UsosMaximos { get; set; }
        public int UsosActuales { get; set; }
        public bool EstaVigente => FechaFin == null || FechaFin >= DateTime.Today;
    }
}
