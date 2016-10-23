using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DormApp.Entities;
using DormApp.Domain.Interfaces;

namespace DormApp.Domain
{
    class HistoryRepository : Repository<History>, IHistoryRepository
    {
        public HistoryRepository(Dormitory_Entities context) : base(context)
        {
        }
        
        //public string GetActiveFlag(History historyItem)
        //{
        //    if (historyItem.addPaymentFlag == true) { return "addPaymentFlag"; }
        //    else if (historyItem.addPriceFlag == true) { return "addPriceFlag"; }
        //    else if (historyItem.changeRoomFlag == true) { return "changeRoomFlag"; }
        //    else if (historyItem.debitAccountFlag == true) { return "debitAccountFlag"; }
        //    else if (historyItem.insertFlag == true) { return "insertFlag"; }
        //    else if (historyItem.moveOutFlag == true) { return "moveOutFlag"; }
        //    else return string.Empty;
        //}

        public IEnumerable<DebitHistory> GetDebitAccountHistory(int dormId)
        {
            //return (from his in GetAll()
            //        where his.dorm_id == dormId && his.debitAccountFlag == true
            //        select new DebitHistory { Admin = his.admin_name, Description = his.description }).ToList();
            return GetAll().Where(his => his.dorm_id == dormId && his.debitAccountFlag == true).Select(
                his => new DebitHistory
                {
                    Admin = his.admin_name,
                    Description = his.description
                }).ToList();
        }

        public IEnumerable<History> GetHistory(int dormId)
        {
            //return (from his in GetAll()
            //        where his.dorm_id == dormId
            //        select his).ToList();
            return GetAll().Where(his => his.dorm_id == dormId).Select(his => his).ToList();
        }
    }
}
