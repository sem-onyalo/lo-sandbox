using LoyaltyOne.Data.Models;

namespace LoyaltyOne.Services
{
    public interface ILocationService
    {
        CityDto GetCity(string name);
    }
}
