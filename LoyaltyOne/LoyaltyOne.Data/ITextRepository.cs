using LoyaltyOne.Data.Models;
using System.Collections.Generic;

namespace LoyaltyOne.Data
{
    public interface ITextRepository
    {
        IEnumerable<TextDto> SelectTextsByName(string name);

        IEnumerable<TextDto> SelectTextsByParentIds(List<int> parentIds);

        void InsertText(TextDto text);
    }
}
