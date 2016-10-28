using DormApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DormApp.Domain.Interfaces
{
    public interface IHistoryRepository : IRepository<History>
    {
        IEnumerable<DebitHistory> GetDebitAccountHistory(int dormId);
        IEnumerable<History> GetHistory(int dormId);
    }
}
