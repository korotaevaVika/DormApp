using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DormApp.Entities;

namespace DormApp.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        int Complete();

        IAgreementRepository Agreements { get; }
        IPriceRepository Prices { get; }
        IAccountRepository Accounts { get; }
        ICountryRepository Countries { get; }
        IDormRepository Dormitories { get; }
        IPersonRepository Persons { get; }
        IRoomTypeRepository RoomTypes { get; }
        IHistoryRepository History { get; }
        IRepository<Payment> Payments { get; }
        IMailRepository Mails { get; }

        IEnumerable<Population> GetPopulation(int dormId);
        IEnumerable<Population> GetPopulation(int dormId, int floor);

        void ChangeRoom(PersonData person, NewPlace place, string admin_name, int dorm_id);
        string InsertNewPerson(NewPerson person, string admin_name, int dorm_id);
        void MovePersonOut(PersonData person, string admin_name, int dorm_id);

        IEnumerable<Person> GetPeopleLivingInRoom(int dormId, int room);
        IEnumerable<Person> GetPeopleLivingInRoom(int dormId, int room, int roomTypeId);
        IEnumerable<string> GetPeopleNamesLivingInRoom(int dormId, int room);
        IEnumerable<string> GetPeopleNamesLivingInRoom(int dormId, int room, int roomTypeId);

        IEnumerable<int> GetLivingRooms(int floor, int dormId);

        int GetPersonId(PersonData person);
        void GetPersonAndDormIdByEmail(string email, out int personId, out int dormId);
        string GetRoomType(int person_id);

        void GetInformation(
            PersonData person,
            out long? passport,
            out string contract,
            out string roomtype,
            out decimal sum
            );

        PersonData GetPersonDataById(int personId);

        bool Register(RegistrationData registrationData);
        bool PersonExists(RegistrationData registrationData);
        PersonData ConvertRegistrationDataToPersonData(RegistrationData registrationData);

        string AddTariff(Tariff tariff, int dormId, string adminName);
        IEnumerable<Tariff> GetTariffs(int dormId);
        IEnumerable<Tariff> GetAllTariffs();

        string GetRoomGender(int dormId, int roomNumber, int roomTypeId);
    }
}
