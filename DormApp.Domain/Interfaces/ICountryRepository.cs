using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DormApp.Entities;

namespace DormApp.Domain.Interfaces
{
    public interface ICountryRepository: IRepository<Country>
    {
        IEnumerable<string> GetCountryNames();
        int GetCountryId(string countryName);
    }
}
