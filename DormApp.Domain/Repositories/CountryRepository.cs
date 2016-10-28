using System.Collections.Generic;
using System.Linq;
using DormApp.Entities;
using DormApp.Domain.Interfaces;

namespace DormApp.Domain
{
    class CountryRepository : Repository<Country>, ICountryRepository
    {
        public CountryRepository(Dormitory_Entities context) : base(context)
        {
        }

        public int GetCountryId(string countryName)
        {
            return _context.Country.Where(z => (z.name.Equals(countryName))).Select(x => x.id).FirstOrDefault();
        }

        public IEnumerable<string> GetCountryNames()
        {
            return _context.Country.Select(x => x.name).ToList<string>();
        }
    }
}
