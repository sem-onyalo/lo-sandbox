using LiteDB;
using LoyaltyOne.Data.Models;
using System.Collections.Generic;

namespace LoyaltyOne.Data
{
    public class TextRepository : Repository, ITextRepository
    {
        public IEnumerable<TextDto> SelectTextsByName(string name)
        {
            using (LiteDatabase db = new LiteDatabase(base.ConnectionString))
            {
                var texts = db.GetCollection<TextDto>("Text");

                return texts.Find(x => x.Name == name);
            }
        }

        public void InsertText(TextDto text)
        {
            using (LiteDatabase db = new LiteDatabase(base.ConnectionString))
            {
                var texts = db.GetCollection<TextDto>("Text");

                texts.Insert(text);

                texts.EnsureIndex(x => x.Name);
            }
        }
    }
}
