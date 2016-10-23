using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DormApp.Entities;

namespace DormApp.Domain.Interfaces
{
    public interface IRoomTypeRepository : IRepository<RoomType>
    {
        IEnumerable<string> GetRoomTypeNames();
        int GetRoomTypeId(string roomName);
        //string GetRoomGender(int dormId, int roomNumber, int roomTypeId);
    }
}
