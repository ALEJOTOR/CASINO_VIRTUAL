namespace ENTITY
{
    public class MetodoPago
    {
        public int IdMetodo { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }

        public override string ToString()
        {
            return Tipo;
        }
    }
}
