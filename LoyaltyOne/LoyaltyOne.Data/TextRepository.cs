using LiteDB;
using LoyaltyOne.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace LoyaltyOne.Data
{
    public class TextRepository : Repository, ITextRepository
    {
        public TextRepository() : base() { }

        public TextRepository(string connectionString) : base(connectionString) { }

        public IEnumerable<TextDto> SelectTextsByName(string name)
        {
            using (LiteDatabase db = new LiteDatabase(base.ConnectionString))
            {
                var texts = db.GetCollection<TextDto>("Text");

                return texts.Find(x => x.Name == name);
            }
        }

        public IEnumerable<TextDto> SelectTextsByParentIds(List<int> parentIds)
        {
            using (LiteDatabase db = new LiteDatabase(base.ConnectionString))
            {
                var texts = db.GetCollection<TextDto>("Text");

                return texts
                    .FindAll()
                    .Where(x => parentIds.Contains(x.ParentId));
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
