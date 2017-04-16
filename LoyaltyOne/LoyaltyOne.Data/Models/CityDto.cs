using System;

namespace LoyaltyOne.Data.Models
{
    public class CityDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Temperature { get; set; }

        public DateTime LastUpdatedDateTimeUtc { get; set; }
    }
}
