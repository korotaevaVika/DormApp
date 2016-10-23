using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DormApp.Entities;
using DormApp.Domain.Interfaces;

namespace DormApp.Domain
{
    class DormRepository : Repository<DormType>, IDormRepository
    {
        public DormRepository(Dormitory_Entities context) : base(context)
        {
        }

        public int GetDormIdByName(string dormName)
        {
            return GetAll().Where(x => x.name == dormName).Select(x => x.id_building).Last();
        }

        public IEnumerable<string> GetDormNames()
        {
            return GetAll().OrderBy(x => x.name).Select(x => x.name);
        }

        public IEnumerable<int> GetLivingFloors(int dorm_id)
        {
            return _context.GetNotEmptyFloors(dorm_id).OrderBy(x => x.Value).Select(x => x.Value).Distinct();
        }

        public int GetMaximumFloor(int dormId)
        {
            return GetAll().Where(dorm => dorm.id_building == dormId).Select(dorm => dorm.floor_number).Last();
        }

        //public IEnumerable<Population> GetPopulation(int dormId)
        //{
        //    IEnumerable<int> _floors = GetLivingFloors(dormId);
        //    List<Population> _population = new List<Population>();
        //    RoomRepository roomRepository = new RoomRepository(_context);
        //    foreach (var floor in _floors)
        //    {
        //        _population.AddRange(_context.GetPopulation(dormId, floor).Select(res =>
        //            new Population
        //            {
        //                RoomType = res.roomName,
        //                Quantity = (int)res.Quantity,
        //                RoomNumber = res.roomNum,
        //                Gender = roomRepository.GetRoomGender(
        //                         dormId,
        //                         res.roomNum,
        //                         roomRepository.GetRoomTypeId(res.roomName))
        //            }).ToList());
        //    }
        //    return _population;

        //    //(from res in _context.GetPopulation(dormId, floor)
        //    // select new Population
        //    // {
        //    //     RoomType = res.roomName,
        //    //     Quantity = (int)res.Quantity,
        //    //     RoomNumber = res.roomNum,
        //    //     Gender = roomRepository.GetRoomGender(
        //    //         dormId,
        //    //         res.roomNum,
        //    //         roomRepository.GetRoomTypeId(res.roomName)
        //    //         )
        //    // }).ToList());
        //}
    }
}
