using System;

namespace ENTITY
{
    public class Transaccion
    {
        public int IdTransaccion { get; set; }
        public int IdUsuario { get; set; }
        public string Tipo { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }

        public override string ToString()
        {
            return string.Join("|",
                IdTransaccion, IdUsuario, Tipo,
                Monto.ToString("F2"),
                Fecha.ToString("yyyy-MM-dd HH:mm:ss"),
                Descripcion ?? "");
        }
    }
}
