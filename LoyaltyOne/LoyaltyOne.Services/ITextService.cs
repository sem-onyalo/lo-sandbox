using System.Collections.Generic;

namespace LoyaltyOne.Services
{
    public interface ITextService
    {
        string PingText(string text);
        
        string SaveText(string text);
    }
}
