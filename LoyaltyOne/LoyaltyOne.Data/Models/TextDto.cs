using System;

namespace LoyaltyOne.Data.Models
{
    public class TextDto
    {
        public int Id { get; set; }

        public int ParentId { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string CityName { get; set; }

        public DateTime CreatedDateTimeUtc { get; set; }
    }
}
