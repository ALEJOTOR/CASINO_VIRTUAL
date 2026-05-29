namespace BLL.Wompi
{
    public interface IWompiSignatureService
    {
        string GenerarFirma(string referencia, long centavos, string moneda);
    }
}
