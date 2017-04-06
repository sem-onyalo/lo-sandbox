using LiteDB;
using LoyaltyOne.Data.Models;
using System.Collections.Generic;

namespace LoyaltyOne.Data
{
    public class TextRepository : Repository, ITextRepository
    {
        public IEnumerable<TextDto> SelectTexts()
        {
            using (LiteDatabase db = new LiteDatabase(base.ConnectionString))
            {
                var texts = db.GetCollection<TextDto>("Text");

                return texts.FindAll();
            }
        }

        public void InsertText(TextDto text)
        {
            using (LiteDatabase db = new LiteDatabase(base.ConnectionString))
            {
                var texts = db.GetCollection<TextDto>("Text");

                texts.Insert(text);
            }
        }
    }
}
