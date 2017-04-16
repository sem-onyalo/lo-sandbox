using LoyaltyOne.Data;
using LoyaltyOne.Data.Models;
using LoyaltyOne.Services.Models;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;

namespace LoyaltyOne.Services
{
    public class LocationService : ILocationService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IApiService _apiService;

        public LocationService(ICityRepository cityRepository, IApiService apiService)
        {
            if (cityRepository == null) throw new ArgumentNullException("cityRepository");
            if (apiService == null) throw new ArgumentNullException("apiService");

            if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["WeatherApiUrl"]))
                throw new MissingFieldException("Weather API URL config value is invalid");

            if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["CityDataTimeoutHrs"]))
                throw new MissingFieldException("City data timeout hours config value is invalid");

            _cityRepository = cityRepository;
            _apiService = apiService;
        }

        public CityDto GetCity(string name)
        {
            string weatherApiUrl = HttpUtility.HtmlDecode(ConfigurationManager.AppSettings["WeatherApiUrl"]);
            int cityDataTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CityDataTimeoutHrs"]);

            CityDto city = _cityRepository.SelectCityByName(name);

            if (city == null || (city != null && city.LastUpdatedDateTimeUtc.AddHours(cityDataTimeout) <= DateTime.UtcNow))
            {
                string getWeatherDataUri = string.Format(weatherApiUrl, name);

                GetWeatherDataResponse apiResponse = _apiService.GetRequest<GetWeatherDataResponse>(getWeatherDataUri);

                if (city == null) city = new CityDto();

                city.Name = name;
                city.Latitude = apiResponse.Coord.Lat;
                city.Longitude = apiResponse.Coord.Lon;
                city.Temperature = apiResponse.Main.Temp;
                city.LastUpdatedDateTimeUtc = DateTime.UtcNow;

                city = _cityRepository.InsertOrUpdateCity(city);
            }

            return city;
        }
    }
}
