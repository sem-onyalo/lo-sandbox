using System;

namespace LoyaltyOne.Data.Models
{
    public class TextDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public DateTime CreatedDateTimeUtc { get; set; }
    }
}
