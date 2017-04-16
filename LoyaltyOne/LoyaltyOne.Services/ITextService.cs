using LoyaltyOne.Data.Models;
using LoyaltyOne.Services.Models;
using System.Collections.Generic;

namespace LoyaltyOne.Services
{
    public interface ITextService
    {
        string PingText(string text);

        GetTextsResponse GetTexts(string name);

        SaveTextResponse SaveText(SaveTextRequest request);
    }
}
