
namespace LoyaltyOne.Services.Models
{
    public class GetWeatherDataResponse
    {
        public int Id { get; set; }

        public string City { get; set; }

        public GetWeatherDataResponseMain Main { get; set; }

        public GetWeatherDataResponseCoord Coord { get; set; }
    }
}
