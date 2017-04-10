using System.Collections.Generic;

namespace LoyaltyOne.WebApi.Models
{
    public class GetTextsResponse
    {
        public string Name { get; set; }

        public IList<string> Texts { get; set; }

        public string Error { get; set; }
    }
}