using System;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Wompi
{
    public class WompiSignatureService : IWompiSignatureService
    {
        private readonly string _integrityKey;

        public WompiSignatureService(string integrityKey)
        {
            _integrityKey = integrityKey ?? "";
        }

        public string GenerarFirma(string referencia, long centavos, string moneda)
        {
            string raw = $"{referencia}{centavos}{moneda}{_integrityKey}";
            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
