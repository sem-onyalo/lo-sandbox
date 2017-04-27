using LiteDB;
using LoyaltyOne.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace LoyaltyOne.Data
{
    public class CityRepository : Repository, ICityRepository
    {
        public CityRepository() : base() { }

        public CityRepository(string connectionString) : base(connectionString) { }

        public CityDto SelectCityByName(string name)
        {
            using (LiteDatabase db = new LiteDatabase(base.ConnectionString))
            {
                var cities = db.GetCollection<CityDto>("City");

                return cities
                    .Find(x => x.Name == name)
                    .FirstOrDefault();
            }
        }

        public IEnumerable<CityDto> SelectCitiesByName(List<string> names)
        {
            using (LiteDatabase db = new LiteDatabase(base.ConnectionString))
            {
                var cities = db.GetCollection<CityDto>("City");

                return cities
                    .FindAll()
                    .Where(x => names.Contains(x.Name));
            }
        }

        public CityDto InsertOrUpdateCity(CityDto city)
        {
            using (LiteDatabase db = new LiteDatabase(base.ConnectionString))
            {
                var cities = db.GetCollection<CityDto>("City");

                cities.Upsert(city);

                cities.EnsureIndex(x => x.Name);

                return city;
            }
        }
    }
}
