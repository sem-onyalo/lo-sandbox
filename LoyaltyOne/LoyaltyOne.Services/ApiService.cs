using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace LoyaltyOne.Services
{
    public class ApiService : IApiService
    {
        public T GetRequest<T>(string uri)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);

                Task<HttpResponseMessage> httpReponse = client.SendAsync(request);
                httpReponse.Wait();

                Task<string> responseContent = httpReponse.Result.Content.ReadAsStringAsync();
                responseContent.Wait();

                return JsonConvert.DeserializeObject<T>(responseContent.Result);
            }
        }
    }
}
