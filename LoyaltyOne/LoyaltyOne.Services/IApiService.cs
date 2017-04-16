using System.Threading.Tasks;

namespace LoyaltyOne.Services
{
    public interface  IApiService
    {
        T GetRequest<T>(string uri);
    }
}
