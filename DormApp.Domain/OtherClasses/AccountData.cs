using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DormApp.Domain
{
    public class AccountData
    {
        public decimal AccountAmount { get; set; }
        public decimal PricePerMonth { get; set; }
        public bool IsStudent { get; set; }
        public bool IsBudget { get; set; }

    }
}
