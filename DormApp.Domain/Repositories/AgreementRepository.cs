using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DormApp.Entities;
using DormApp.Domain.Interfaces;
namespace DormApp.Domain
{
    class AgreementRepository : Repository<Agreement>, IAgreementRepository
    {
        public AgreementRepository(Dormitory_Entities context) : base(context)
        {
        }

        public int GetRoomId(int person_id)
        {
            return GetAll().Where(a => (a.person_id == person_id) && (a.is_active == true)).Select(a => a.room_id).ToList().Last();
        }
    }
}
