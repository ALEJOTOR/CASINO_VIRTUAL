using System;

namespace ENTITY
{
    public class TransaccionDisplayDto
    {
        public int IdTransaccion { get; set; }
        public string Usuario { get; set; }
        public string Tipo { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
    }
}
