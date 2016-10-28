using System.Collections.Generic;
using System.Linq;
using DormApp.Entities;
using DormApp.Domain.Interfaces;

namespace DormApp.Domain
{
    class RoomRepository : Repository<RoomType>, IRoomTypeRepository
    {
        public RoomRepository(Dormitory_Entities context) : base(context)
        {
        }

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
