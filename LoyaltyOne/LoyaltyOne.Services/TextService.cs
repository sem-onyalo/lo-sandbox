using System;

namespace LoyaltyOne.Services
{
    public class TextService : ITextService
    {
        /// <summary>
        /// Returns the text passed to it.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public string PingText(string text)
        {
            return text != null ? text : string.Empty;
        }
    }
}
