using LoyaltyOne.Data.Models;
using System.Collections.Generic;

namespace LoyaltyOne.Services.Models
{
    public class GetTextsResponse
    {
        public IList<TextDto> Texts { get; set; }

        public IList<CityDto> Cities { get; set; }
    }
}
