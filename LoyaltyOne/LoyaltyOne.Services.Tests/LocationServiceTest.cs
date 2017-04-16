using LoyaltyOne.Data;
using LoyaltyOne.Data.Models;
using LoyaltyOne.Services.Models;
using Moq;
using NUnit.Framework;
using System;

namespace LoyaltyOne.Services.Tests
{
    [TestFixture]
    public class LocationServiceTest
    {
        private Mock<ICityRepository> _cityRepository;
        private Mock<IApiService> _apiService;
        private ILocationService _locationService;

        [SetUp]
        public void TestSetUp()
        {
            _cityRepository = new Mock<ICityRepository>();
            _apiService = new Mock<IApiService>();
            _locationService = new LocationService(_cityRepository.Object, _apiService.Object);
        }

        [Test]
        public void ConstructorTest_ShouldNotBeNull()
        {
            Assert.IsNotNull(_locationService);
        }

        [Test]
        public void GetCity_ShouldReturnCityFromRepo()
        {
            CityDto city = new CityDto();
            city.Name = "London";
            city.LastUpdatedDateTimeUtc = DateTime.UtcNow;

            _cityRepository
                .Setup(x => x.SelectCityByName(city.Name))
                .Returns(city);

            CityDto actual = _locationService.GetCity(city.Name);

            Assert.IsNotNull(actual);
            Assert.AreEqual(city.Name, actual.Name);
        }

        [Test]
        public void GetCity_ShouldReturnCityFromApiIfCityFromRepoIsNull()
        {
            CityDto city = null;

            GetWeatherDataResponse getWeatherDataResponse = new GetWeatherDataResponse();
            getWeatherDataResponse.Id = 54321;
            getWeatherDataResponse.City = "London";
            getWeatherDataResponse.Coord = new GetWeatherDataResponseCoord { Lat = 50, Lon = -50 };
            getWeatherDataResponse.Main = new GetWeatherDataResponseMain { Temp = 26 };

            _cityRepository
                .Setup(x => x.SelectCityByName("London"))
                .Returns(city);

            _apiService
                .Setup(x => x.GetRequest<GetWeatherDataResponse>("http://api.weather.com/weather?city=London"))
                .Returns(getWeatherDataResponse);

            _cityRepository
                .Setup(x => x.InsertOrUpdateCity(It.Is<CityDto>(y => y.Name == "London")))
                .Returns(new CityDto
                {
                    Id = 1,
                    Name = getWeatherDataResponse.City,
                    Latitude = getWeatherDataResponse.Coord.Lat,
                    Longitude = getWeatherDataResponse.Coord.Lon,
                    Temperature = getWeatherDataResponse.Main.Temp
                });

            CityDto actual = _locationService.GetCity("London");

            Assert.IsNotNull(actual);
            Assert.AreEqual(getWeatherDataResponse.City, actual.Name);
            Assert.AreEqual(getWeatherDataResponse.Coord.Lat, actual.Latitude);
            Assert.AreEqual(getWeatherDataResponse.Coord.Lon, actual.Longitude);
            Assert.AreEqual(getWeatherDataResponse.Main.Temp, actual.Temperature);
        }

        [Test]
        public void GetCity_ShouldReturnCityFromApiIfCityFromRepoIsExpired()
        {
            CityDto city = new CityDto();
            city.Name = "London";
            city.Temperature = 20;
            city.LastUpdatedDateTimeUtc = DateTime.UtcNow.AddHours(-2);

            GetWeatherDataResponse getWeatherDataResponse = new GetWeatherDataResponse();
            getWeatherDataResponse.Id = 54321;
            getWeatherDataResponse.City = "London";
            getWeatherDataResponse.Coord = new GetWeatherDataResponseCoord { Lat = 50, Lon = -50 };
            getWeatherDataResponse.Main = new GetWeatherDataResponseMain { Temp = 26 };

            _cityRepository
                .Setup(x => x.SelectCityByName("London"))
                .Returns(city);

            _apiService
                .Setup(x => x.GetRequest<GetWeatherDataResponse>("http://api.weather.com/weather?city=London"))
                .Returns(getWeatherDataResponse);

            _cityRepository
                .Setup(x => x.InsertOrUpdateCity(It.Is<CityDto>(y => y.Name == "London")))
                .Returns(new CityDto
                {
                    Id = 1,
                    Name = getWeatherDataResponse.City,
                    Latitude = getWeatherDataResponse.Coord.Lat,
                    Longitude = getWeatherDataResponse.Coord.Lon,
                    Temperature = getWeatherDataResponse.Main.Temp
                });

            CityDto actual = _locationService.GetCity("London");

            Assert.IsNotNull(actual);
            Assert.AreEqual(getWeatherDataResponse.Main.Temp, actual.Temperature);
        }
    }
}
