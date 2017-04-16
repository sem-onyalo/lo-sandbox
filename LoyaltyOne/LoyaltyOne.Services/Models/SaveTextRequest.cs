using LoyaltyOne.Data.Models;

namespace LoyaltyOne.Services.Models
{
    public class SaveTextRequest
    {
        public TextDto Text { get; set; }

        public string City { get; set; }
    }
}
