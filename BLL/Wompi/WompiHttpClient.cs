using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLL.Wompi
{
    public class WompiHttpClient : IWompiHttpClient
    {
        private static readonly HttpClient _http = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        public async Task<HttpResponseMessage> GetAsync(string endpoint, string authToken)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, endpoint))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
                return await _http.SendAsync(request);
            }
        }

        public async Task<HttpResponseMessage> PostAsync(string endpoint, string authToken, object payload)
        {
            string json = JsonSerializer.Serialize(payload,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower });

            using (var request = new HttpRequestMessage(HttpMethod.Post, endpoint))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                return await _http.SendAsync(request);
            }
        }
    }
}
