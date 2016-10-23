using DormApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DormApp.Domain.Interfaces;

namespace DormApp.Domain
{
    class PriceRepository : Repository<PriceList>, IPriceRepository
    {
        public PriceRepository(Dormitory_Entities context) : base(context)
        {
        }

        //public string Add(Tariff tariff, int dormId, string adminName)
        //{
        //    RoomRepository roomPepository = new RoomRepository(_context);
        //    try
        //    {
        //        bool is_active = (
        //            ((tariff.DateFinish.HasValue == true) && (tariff.DateStart <= DateTime.Now.Date) &&
        //            (tariff.DateFinish >= DateTime.Now.Date))
        //            || ((tariff.DateFinish.HasValue == false) && (tariff.DateStart <= DateTime.Now.Date))) ? true : false;

        //        PriceList priceList = new PriceList
        //        {
        //            title = tariff.Title,
        //            price = tariff.Price,
        //            room_id = roomPepository.GetRoomTypeId(tariff.RoomTypeName),
        //            is_student = tariff.IsStudent,
        //            on_budget = tariff.OnBudget,
        //            date_start = tariff.DateStart.Date,
        //            date_end = tariff.DateFinish.HasValue ? tariff.DateFinish.Value.Date : new Nullable<DateTime>(),
        //            dorm_id = dormId,
        //            is_active = is_active
        //        };

        //        if (isPriceListExisted(priceList) == false)
        //        {
        //            Add(priceList);
        //            HistoryRepository historyRepository = new HistoryRepository(_context);
        //            historyRepository.Add(new History
        //            {
        //                admin_name = adminName,
        //                dorm_id = dormId,
        //                description = "Добавлен тариф, id которого " + GetAll().Select(x => x.id).Last(), 
        //                addPriceFlag = true
        //            });
        //            return string.Empty;
        //        }
        //        else
        //        {
        //            return "Такой тариф уже есть в базе";
        //        };

        //    }
        //    catch (Exception ex) { return ex.ToString(); }
        //}

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

        //public IEnumerable<Tariff> GetTariffs(int dormId)
        //{
        //    IRoomTypeRepository roomTypeRepository = new RoomRepository(_context);

        //    return GetAll().Where(p => p.dorm_id == dormId).Select(p => new Tariff
        //    {
        //        Title = p.title,
        //        RoomTypeName = roomTypeRepository.Get(p.room_id).name,
        //        DateStart = p.date_start,
        //        DateFinish = p.date_end,
        //        IsStudent = p.is_student,
        //        OnBudget = p.on_budget,
        //        Price = p.price
        //    }
        //            ).ToList();
        //}


    }
}
