using LoyaltyOne.Data.Models;
using System.Collections.Generic;

namespace LoyaltyOne.Data
{
    public interface ICityRepository
    {
        CityDto SelectCityByName(string name);

        IEnumerable<CityDto> SelectCitiesByName(List<string> names);

        CityDto InsertOrUpdateCity(CityDto city);
    }
}
