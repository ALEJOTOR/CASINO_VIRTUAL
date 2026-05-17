namespace ENTITY
{
    public class EstadoPartida
    {
        public int IdEstado { get; set; }
        public string NombreEstado { get; set; }
        public string Descripcion { get; set; }

        public override string ToString()
        {
            return $"{IdEstado}|{NombreEstado}|{Descripcion}";
        }
    }
}
