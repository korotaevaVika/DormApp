﻿using DormApp.Entities;
using System.Linq;
using DormApp.Domain.Interfaces;

namespace DormApp.Domain
{
    class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(Dormitory_Entities context) : base(context)
        {
        }

        public decimal GetSumOnAccount(int person_id)
        {
            try
            {
                return GetAll().Where(a => (a.id_person == person_id)).Select(a => a.sum).ToList().LastOrDefault();
            }
            catch { return 0.0m; }
        }
    }
}
