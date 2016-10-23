using DormApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DormApp.Domain.Interfaces
{
    public interface IPriceRepository: IRepository<PriceList>
    {
        //IEnumerable<Tariff> GetTariffs(int dormId);
        bool isPriceListExisted(PriceList priceList);

        //string Add(Tariff tariff, int dormId, string adminName);
    }
}
