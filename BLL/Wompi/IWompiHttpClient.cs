using System.Net.Http;
using System.Threading.Tasks;

namespace BLL.Wompi
{
    public interface IWompiHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string endpoint, string authToken);
        Task<HttpResponseMessage> PostAsync(string endpoint, string authToken, object payload);
    }
}
