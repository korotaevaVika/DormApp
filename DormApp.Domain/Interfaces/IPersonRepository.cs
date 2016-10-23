using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DormApp.Entities;

namespace DormApp.Domain.Interfaces
{
    public interface IPersonRepository: IRepository<Person>
    {
        //int GetPersonId(PersonData person);

        //IEnumerable<Person> GetPeopleLivingInRoom(int dormId, int room);
        //IEnumerable<Person> GetPeopleLivingInRoom(int dormId, int room, int roomTypeId);
        //IEnumerable<string> GetPeopleNamesLivingInRoom(int dormId, int room);
        //IEnumerable<string> GetPeopleNamesLivingInRoom(int dormId, int room, int roomTypeId);

        long? GetPassport(int person_id);
        string GetContract(int person_id);
        ////int GetRoomId(int person_id);
        //string GetRoomType(int person_id);
        //decimal GetSumOnAccount(int person_id);

        //void GetInformation(
        //    PersonData person,
        //    out long? passport,
        //    out string contract,
        //    out string roomtype,
        //    out decimal sum
        //    );

        //void MovePersonOut(PersonData person, string admin_name, int dorm_id);
        //void ChangeRoom(PersonData person, NewPlace place, string admin_name, int dorm_id);
        //string InsertNewPerson(NewPerson person, string admin_name, int dorm_id);
    }
}
