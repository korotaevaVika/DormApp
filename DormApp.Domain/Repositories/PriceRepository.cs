using DormApp.Entities;
using System.Linq;
using DormApp.Domain.Interfaces;

namespace DormApp.Domain
{
    class PriceRepository : Repository<PriceList>, IPriceRepository
    {
        public PriceRepository(Dormitory_Entities context) : base(context)
        {
        }

        public bool isPriceListExisted(PriceList priceList)
        {
            if (Find(p =>
            p.date_end == priceList.date_end &&
            p.date_start == priceList.date_start &&
            p.dorm_id == priceList.dorm_id &&
            p.is_active == priceList.is_active &&
            p.is_student == priceList.is_student &&
            p.on_budget == priceList.on_budget &&
            p.room_id == priceList.room_id &&
            p.price == priceList.price &&
            p.title == priceList.title
            ).Count() == 0)
            { return false; }
            else { return true; }
        }
    }
}
