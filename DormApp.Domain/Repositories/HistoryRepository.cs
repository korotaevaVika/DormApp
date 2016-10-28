using System.Collections.Generic;
using System.Linq;
using DormApp.Entities;
using DormApp.Domain.Interfaces;

namespace DormApp.Domain
{
    class HistoryRepository : Repository<History>, IHistoryRepository
    {
        public HistoryRepository(Dormitory_Entities context) : base(context)
        {
        }

        public IEnumerable<DebitHistory> GetDebitAccountHistory(int dormId)
        {
            return GetAll().Where(his => his.dorm_id == dormId && his.debitAccountFlag == true).Select(
                his => new DebitHistory
                {
                    Admin = his.admin_name,
                    Description = his.description
                }).ToList();
        }

        public IEnumerable<History> GetHistory(int dormId)
        {
            return GetAll().Where(his => his.dorm_id == dormId).Select(his => his).ToList();
        }
    }
}
