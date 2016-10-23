using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DormApp.Entities;
using DormApp.Domain.Interfaces;
using System.Diagnostics;

namespace DormApp.Domain
{
    class PersonRepository : Repository<Person>, IPersonRepository
    {
        public PersonRepository(Dormitory_Entities context) : base(context)
        {
        }

        //public void ChangeRoom(PersonData person, NewPlace place, string admin_name, int dorm_id)
        //{
        //    AgreementRepository agreementRepository = new AgreementRepository(_context);

        //    (from a in agreementRepository.GetAll()             
        //     where a.person_id == GetPersonId(person)
        //     select a).ToList().
        //     ForEach(x =>
        //     {
        //         x.floor_number = place.NewFloor;
        //         x.room_number = place.NewRoom;
        //         x.room_id = place.NewRoomTypeId;
        //     });

        //    HistoryRepository _history = new HistoryRepository(_context);
        //    _history.Add(new History
        //    {
        //        admin_name = admin_name,
        //        dorm_id = dorm_id,
        //        description = person.SurName + " " + person.Name + " " + "переехал(-а) из " + person.Room
        //        + " в " + place.NewRoom, 
        //        changeRoomFlag = true
        //    });
        //}

        public string GetContract(int person_id)
        {
            return GetAll().Where(p => p.id_person == person_id && (p.is_living == true)).Select(p => p.contract).LastOrDefault();
        }

        //public void GetInformation(
        //    PersonData person,
        //    out long? passport,
        //    out string contract,
        //    out string roomtype,
        //    out decimal sum
        //    )
        //{
        //    int person_id = GetPersonId(person);
        //    passport = GetPassport(person_id);
        //    contract = GetContract(person_id);
        //    roomtype = GetRoomType(person_id);
        //    sum = GetSumOnAccount(person_id);
        //}

        //public IEnumerable<Person> GetPeopleLivingInRoom(int dormId, int room)
        //{
        //    return (from p in GetAll()//personrep
        //            join a in _context.Agreement
        //            on p.id_person equals a.person_id
        //            where
        //            //(a.floor_number == floor) && 
        //            (p.is_living == true)
        //            && (a.room_number == room) && (a.dorm_id == dormId)
        //            select p).ToList();
        //}

        //public IEnumerable<Person> GetPeopleLivingInRoom(int dormId, int room, int roomTypeId)
        //{
        //    return (from p in GetAll()
        //            join a in _context.Agreement
        //            on p.id_person equals a.person_id
        //            where //(a.floor_number == floor) &&
        //            (p.is_living == true)
        //            && (a.room_number == room) && (a.dorm_id == dormId) && (a.room_id == roomTypeId)
        //            select p).ToList();
        //}

        public long? GetPassport(int person_id)
        {
            return GetAll().Where(p => (p.id_person == person_id) && (p.is_living == true))
                .Select(p => p.passport)
                .ToList()
                .LastOrDefault();
            //return (from p in GetAll()
            //        where (p.id_person == person_id) && (p.is_living == true)
            //        select p.passport).ToList().LastOrDefault();
        }

        //public int GetRoomId(int person_id)
        //{
        //    return (from a in _context.Agreement
        //            where (a.person_id == person_id) && (a.is_active == true)
        //            select a.room_id).ToList().Last();
        //}

        //public string GetRoomType(int person_id)
        //{
        //    try
        //    {
        //        return (from r in _context.RoomType
        //                join a in _context.Agreement
        //                on r.id_type equals a.room_id
        //                where (a.person_id == person_id)
        //                select r.name).ToList().Last<string>();
        //    }
        //    catch { return "Комнатка"; }
        //}

        //public decimal GetSumOnAccount(int person_id)
        //{
        //    try
        //    {
        //        return (from a in _context.Account
        //                where (a.id_person == person_id)
        //                select a.sum).ToList().LastOrDefault();
        //    }
        //    catch { return 0.0m; }
        //}

        //public void MovePersonOut(PersonData person, string admin_name, int dorm_id)
        //{
        //    (from p in GetAll()
        //     where p.id_person == GetPersonId(person)
        //     select p).ToList().ForEach(x => { x.is_living = false; });

        //    AgreementRepository agreementRepository = new AgreementRepository(_context);
        //    (from a in agreementRepository.GetAll()
        //     where a.person_id == GetPersonId(person)
        //     select a).ToList().ForEach(x => { x.is_active = false; });

        //    HistoryRepository _history = new HistoryRepository(_context);
        //    _history.Add(new History
        //    {
        //        admin_name = admin_name,
        //        dorm_id = dorm_id,
        //        description = person.SurName + " " + person.Name + " " + "выселен(-а) из " + person.Room, 
        //        moveOutFlag = true
        //    });

        //}

        //public int GetPersonId(PersonData person)
        //{
        //    try
        //    {
        //        return (from p in _context.Person
        //                join a in _context.Agreement
        //                on p.id_person equals a.person_id
        //                where ((p.surname == person.SurName) && (p.name == person.Name) && (p.is_living == true) &&
        //                (a.room_number == person.Room) && (a.dorm_id == person.DormID))
        //                //&&
        //                //(!(person.RoomType.Equals(null)) && (person.RoomType == a.room_id)
        //                //|| (person.RoomType.Equals(null)))
        //                select p.id_person).ToList().Last();
        //    }
        //    catch { return 0; }

        //}

        //public string InsertNewPerson(NewPerson person, string admin_name, int dorm_id)
        //{
        //    CountryRepository _countryRep = new CountryRepository(_context);
        //    RoomRepository _roomRep = new RoomRepository(_context);
        //    _context.InsertPerson(
        //        person.SurName,
        //        person.Name,
        //        person.LastName,
        //        person.Passport,
        //        person.IsMale,
        //        person.IsStudent,
        //        person.IsBudget,
        //        _countryRep.GetCountryId(person.Country),
        //        person.Contract,
        //        dorm_id,
        //        person.Floor,
        //        person.Room,
        //        _roomRep.GetRoomTypeId(person.RoomTypeName),
        //        person.DateStart,
        //        person.DateFinish
        //        );

        //    string description = "Добавление " + person.SurName + " " + person.Name + " " +
        //        person.Passport + " " + person.Country + ". Комната:" + " " + person.Room + " " + person.RoomTypeName;

        //    HistoryRepository _history = new HistoryRepository(_context);
        //    _history.Add(new History
        //    {
        //        admin_name = admin_name,
        //        dorm_id = dorm_id,
        //        description = description,
        //        insertFlag = true
        //    });

        //    return description;
        //}
    }
}
