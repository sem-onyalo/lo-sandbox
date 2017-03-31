
namespace LoyaltyOne.Services
{
    public interface ITextService
    {
        /// <summary>
        /// Returns the text passed to it.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        string PingText(string text);
    }
}
