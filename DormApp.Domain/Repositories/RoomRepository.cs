using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DormApp.Entities;
using DormApp.Domain.Interfaces;

namespace DormApp.Domain
{
    class RoomRepository : Repository<RoomType>, IRoomTypeRepository
    {
        public RoomRepository(Dormitory_Entities context) : base(context)
        {
        }

        //public string GetRoomGender(int dormId, int roomNumber, int roomTypeId)
        //{
        //    List<bool> isMale;
        //    isMale = (from p in _context.Person
        //              join a in _context.Agreement
        //              on p.id_person equals a.person_id
        //              where
        //              (p.is_living == true) && (a.room_id == roomTypeId) && (a.is_active == true)
        //              && (a.room_number == roomNumber) && (a.dorm_id == dormId)
        //              select p.is_male).ToList();

        //    bool first = isMale.First(); //true, if there is a man in room
        //    bool q = true; //true, if all the elements the same
        //    foreach (bool item in isMale)
        //    {
        //        if (item != first)
        //        {
        //            q = false;
        //            break;
        //        }
        //    }
        //    if (!q) { return "М/Ж"; }
        //    else if (first) { return "М"; }
        //    else { return "Ж"; }
        //}

        public int GetRoomTypeId(string roomName)
        {
            return GetAll().Where(r => r.name == roomName).Select(r => r.id_type).ToList().LastOrDefault();
        }

        public IEnumerable<string> GetRoomTypeNames()
        {
            return GetAll().Select(x => x.name).ToList();
        }
    }
}
