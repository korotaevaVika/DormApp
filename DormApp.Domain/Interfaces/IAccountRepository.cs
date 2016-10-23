using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DormApp.Entities;

namespace DormApp.Domain.Interfaces
{
    public interface IAccountRepository: IRepository<Account>
    {
        decimal GetSumOnAccount(int person_id);
    }
}
