using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DormApp.Entities;

namespace DormApp.Domain.Interfaces
{
    public interface IDormRepository : IRepository<DormType>
    {
        IEnumerable<string> GetDormNames();
        //int GetDefaultDormId();
        IEnumerable<int> GetLivingFloors(int dormId);
        //IEnumerable<int> GetLivingFloors(); // on chosen dorm
        //IEnumerable<int> GetLivingRooms(int floor);
        //int GetMaximumFloor(string dormName);
        int GetMaximumFloor(int dormId);
        int GetDormIdByName(string dormName);
        //IEnumerable<Population> GetPopulation(int dormId);
        //IEnumerable<Population> GetPopulation(int dormId, int floor);
    }
}
