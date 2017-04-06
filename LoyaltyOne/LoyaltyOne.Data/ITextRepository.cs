using LoyaltyOne.Data.Models;
using System.Collections.Generic;

namespace LoyaltyOne.Data
{
    public interface ITextRepository
    {
        IEnumerable<TextDto> SelectTexts();

        void InsertText(TextDto text);
    }
}
