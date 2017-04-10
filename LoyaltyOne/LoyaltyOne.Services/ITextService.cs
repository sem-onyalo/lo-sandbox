using LoyaltyOne.Data.Models;
using System.Collections.Generic;

namespace LoyaltyOne.Services
{
    public interface ITextService
    {
        string PingText(string text);

        IList<string> GetTexts(string name);

        TextDto SaveText(TextDto text);
    }
}
