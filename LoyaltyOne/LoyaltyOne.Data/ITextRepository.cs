using LoyaltyOne.Data.Models;
using System.Collections.Generic;

namespace LoyaltyOne.Data
{
    public interface ITextRepository
    {
        IEnumerable<TextDto> SelectTextsByName(string name);

        void InsertText(TextDto text);
    }
}
