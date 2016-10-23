using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DormApp.Entities;

namespace DormApp.Domain
{
    class PaymentRepository: Repository<Payment>
    {
        public PaymentRepository(Dormitory_Entities context):base (context)
        {
        }
    }
}
