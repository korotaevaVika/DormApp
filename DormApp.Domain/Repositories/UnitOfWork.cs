using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DormApp.Domain.Interfaces;
using DormApp.Entities;
using System.Diagnostics;

namespace DormApp.Domain
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dormitory_Entities _context;

        public IAccountRepository Accounts { get; private set; }
        public ICountryRepository Countries { get; private set; }
        public IDormRepository Dormitories { get; private set; }
        public IPersonRepository Persons { get; private set; }
        public IRoomTypeRepository RoomTypes { get; private set; }
        public IHistoryRepository History { get; private set; }
        public IAgreementRepository Agreements { get; private set; }
        public IPriceRepository Prices { get; private set; }
        public IRepository<Payment> Payments { get; private set; }
        public IMailRepository Mails { get; private set; }


        public UnitOfWork(Dormitory_Entities context)
        {
            if (_context == null)
            {
                _context = context;
                Accounts = new AccountRepository(_context);
                Countries = new CountryRepository(_context);
                Dormitories = new DormRepository(_context);
                Persons = new PersonRepository(_context);
                RoomTypes = new RoomRepository(_context);
                History = new HistoryRepository(_context);
                Agreements = new AgreementRepository(_context);
                Prices = new PriceRepository(_context);
                Payments = new PaymentRepository(_context);
                Mails = new MailRepository(_context);
            }
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public string AddTariff(Tariff tariff, int dormId, string adminName)
        {
            try
            {
                bool is_active = (
                    ((tariff.DateFinish.HasValue == true) && (tariff.DateStart <= DateTime.Now.Date) &&
                    (tariff.DateFinish >= DateTime.Now.Date))
                    || ((tariff.DateFinish.HasValue == false) && (tariff.DateStart <= DateTime.Now.Date))) ? true : false;

                PriceList priceList = new PriceList
                {
                    title = tariff.Title,
                    price = tariff.Price,
                    room_id = RoomTypes.GetRoomTypeId(tariff.RoomTypeName),
                    is_student = tariff.IsStudent,
                    on_budget = tariff.OnBudget,
                    date_start = tariff.DateStart.Date,
                    date_end = tariff.DateFinish.HasValue ? tariff.DateFinish.Value.Date : new DateTime?(),
                    dorm_id = dormId,
                    is_active = is_active
                };

                if (Prices.isPriceListExisted(priceList) == false)
                {
                    Prices.Add(priceList);
                    History.Add(new History
                    {
                        admin_name = adminName,
                        dorm_id = dormId,
                        description = "Добавлен тариф, id которого " + Prices.GetAll().Select(x => x.id).Last(),
                        addPriceFlag = true
                    });
                    return string.Empty;
                }
                else
                {
                    return "Такой тариф уже есть в базе";
                };

            }
            catch (Exception ex) { return ex.ToString(); }
        }

        public IEnumerable<Tariff> GetTariffs(int dormId)
        {

            return Prices.GetAll()
                .Where(p => p.dorm_id == dormId)
                .Select(p => new Tariff
                {
                    Title = p.title,
                    RoomTypeName = RoomTypes.Get(p.room_id).name,
                    DateStart = p.date_start,
                    DateFinish = p.date_end,
                    IsStudent = p.is_student,
                    OnBudget = p.on_budget,
                    Price = p.price
                }).ToList();
        }

        public IEnumerable<Tariff> GetAllTariffs()
        {

            return Prices.GetAll()
                .Select(p => new Tariff
                {
                    Title = p.title,
                    RoomTypeName = RoomTypes.Get(p.room_id).name,
                    DateStart = p.date_start,
                    DateFinish = p.date_end,
                    IsStudent = p.is_student,
                    OnBudget = p.on_budget,
                    Price = p.price,
                    DormId = p.dorm_id
                }).ToList();
        }

        public bool DebitAccount(int dormId, int floor, string Admin)
        {
            try
            {
                List<int> personIds = Persons.GetAll().Join(
                    Agreements.GetAll(),
                    person => person.id_person,
                    agreement => agreement.person_id,
                    (person, agreement) => new KeyValuePair<Person, Agreement>(person, agreement))
                    .Where(res => (res.Value.is_active == true) && (res.Value.dorm_id == dormId) &&
                                       (res.Value.floor_number == floor) && (res.Key.is_living == true))
                                       .Select(res => res.Key.id_person)
                                       .ToList();
                foreach (var personId in personIds)
                {
                    Accounts.GetAll().
                       Where(ac => ac.id_person == personId).
                       Select(ac => ac).
                       ToList().
                       ForEach(ac => ac.sum -= GetSuitableTariff(personId, dormId).PricePerMonth);
                }
                History.Add(new History
                {
                    admin_name = Admin,
                    debitAccountFlag = true,
                    dorm_id = dormId,
                    description = DateTime.Now.Date.ToShortDateString() +
                    ". Списание со счетов суммы, должной к уплате, проживающих " + floor + " этажа"
                });
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("------------- \nUnitOfWork \n DebitAccount \n" + ex.ToString() + "\n\n");
                return false;
            }
        }

        public string AddPayment(
            int person_id,
            decimal sum,
            DateTime dateOfPayment,
            string admin,
            int dormId
            )
        {
            try
            {
                Payments.Add(new Payment
                {
                    person_id = person_id,
                    sum = sum,
                    date_payment = dateOfPayment
                });

                Accounts.GetAll().
                    Where(account => account.id_person == person_id).
                    Select(x => x).ToList().ForEach(a => a.sum += sum);

                string personName = Persons.GetAll().Where(p => p.id_person == person_id).
                    Select(p => p.surname + " " + p.name).ToList().Last();

                string description = "Успешно проведен платеж на " + sum.ToString() + " от " + personName +
                    ". Администратор " + Dormitories.Get(dormId).name + " - " + admin + ". " + dateOfPayment.ToString();

                History.Add(new History
                {
                    admin_name = admin,
                    dorm_id = dormId,
                    description = description,
                    addPaymentFlag = true
                });

                return description;
            }
            catch { return "Что-то пошло не так."; }
        }

        public AccountData GetSuitableTariff(int person_id, int dormId)
        {
            try
            {
                bool is_student, on_budget;
                bool? on_budgetNull;
                int roomId;
                decimal amount, price;

                is_student = Persons.GetAll()
                    .Where(per => per.id_person == person_id)
                    .Select(per => per.is_student)
                    .ToList().Last();

                on_budgetNull = Persons.GetAll()
                    .Where(per => per.id_person == person_id)
                    .Select(per => per.on_budget)
                    .ToList()
                    .Last();

                on_budget = on_budgetNull.HasValue ? on_budgetNull.Value : false;

                roomId = Agreements.GetAll()
                    .Where(
                    agr => (agr.person_id == person_id) && (agr.dorm_id == dormId) && (agr.is_active == true))
                    .Select(agr => agr.room_id)
                    .ToList()
                    .Last();

                price = Prices.GetAll()
                    .Where(
                    p => (p.is_student == is_student &&
                         p.on_budget == on_budget && p.is_active == true &&
                         p.dorm_id == dormId && p.room_id == roomId)).
                         Select(p => p.price)
                         .ToList()
                         .Last();

                amount = Accounts.GetAll()
                    .Where(account => account.id_person == person_id)
                    .Select(account => account.sum)
                    .ToList()
                    .Last();

                return new AccountData
                {
                    IsStudent = is_student,
                    IsBudget = on_budget,
                    AccountAmount = amount,
                    PricePerMonth = price
                };
            }
            catch (Exception ex) { Debug.WriteLine(ex.ToString()); return new AccountData(); }
        }

        public IEnumerable<Population> GetPopulation(int dormId)
        {
            IEnumerable<int> _floors = Dormitories.GetLivingFloors(dormId);
            List<Population> _population = new List<Population>();

            foreach (var floor in _floors)
            {
                _population.AddRange(_context.GetPopulation(dormId, floor).Select(res =>
                    new Population
                    {
                        RoomType = res.roomName,
                        Quantity = (int)res.Quantity,
                        RoomNumber = res.roomNum,
                        Gender = GetRoomGender(
                                 dormId,
                                 res.roomNum,
                                 RoomTypes.GetRoomTypeId(res.roomName))
                    }).ToList());
            }
            return _population;
        }

        public IEnumerable<Population> GetPopulation(int dormId, int floor)
        {
            return _context.GetPopulation(dormId, floor).Select(
                res => new Population
                {
                    RoomType = res.roomName,
                    Quantity = (int)res.Quantity,
                    RoomNumber = res.roomNum,
                    Gender = GetRoomGender(
                            dormId,
                            res.roomNum,
                            RoomTypes.GetRoomTypeId(res.roomName)
                            )
                }
                ).ToList();
        }

        public IEnumerable<Person> GetPeopleLivingInRoom(int dormId, int room)
        {
            return Persons.GetAll().Join(Agreements.GetAll(),
                person => person.id_person,
                agreement => agreement.person_id,
                (person, agreement) => new KeyValuePair<Person, Agreement>(person, agreement)
                ).Where(res => res.Key.is_living == true && (res.Value.room_number == room) && (res.Value.dorm_id == dormId)
                ).Select(res => res.Key).ToList();
        }

        public IEnumerable<Person> GetPeopleLivingInRoom(int dormId, int room, int roomTypeId)
        {
            return Persons.GetAll().Join(Agreements.GetAll(),
                person => person.id_person,
                agreement => agreement.person_id,
                (person, agreement) => new KeyValuePair<Person, Agreement>(person, agreement)
                ).Where(res => res.Key.is_living == true && (res.Value.room_number == room) &&
                (res.Value.dorm_id == dormId) && (res.Value.room_id == roomTypeId)
                ).Select(res => res.Key).ToList();
        }

        public IEnumerable<string> GetPeopleNamesLivingInRoom(int dormId, int room)
        {
            return GetPeopleLivingInRoom(dormId, room)
                .Select(p => p.surname + " " + p.name + " " + p.last_name).ToList();
        }

        public IEnumerable<string> GetPeopleNamesLivingInRoom(int dormId, int room, int roomTypeId)
        {
            return GetPeopleLivingInRoom(dormId, room, roomTypeId)
                .Select(p => p.surname + " " + p.name + " " + p.last_name).ToList();
        }

        public void GetPersonAndDormIdByEmail(string email, out int personId, out int dormId)
        {
            int _personId = Mails.GetAll().Where(x => x.email == email).Select(x => x.person_id).SingleOrDefault();
            personId = _personId;
            dormId = Agreements.GetAll().Where(x => (x.person_id == _personId)).Select(x => x.dorm_id).SingleOrDefault();
        }

        public int GetPersonId(PersonData person)
        {
            try
            {
                return Persons.GetAll().Join(
                    Agreements.GetAll(),
                    p => p.id_person,
                    a => a.person_id,
                    (p, a) => new KeyValuePair<Person, Agreement>(p, a)).Where(
                    x => (x.Key.surname == person.SurName) &&
                    (x.Key.name == person.Name) &&
                    (x.Key.is_living == true) &&
                    (x.Value.room_number == person.Room) &&
                    (x.Value.dorm_id == person.DormID)).Select(res => res.Key.id_person).ToList().SingleOrDefault();

                //(from p in _context.Person
                // join a in _context.Agreement
                // on p.id_person equals a.person_id
                // where ((p.surname == person.SurName) && (p.name == person.Name) && (p.is_living == true) &&
                // (a.room_number == person.Room) && (a.dorm_id == person.DormID))
                // //&&
                // //(!(person.RoomType.Equals(null)) && (person.RoomType == a.room_id)
                // //|| (person.RoomType.Equals(null)))
                // select p.id_person).ToList().Last();
            }
            catch { return 0; }
        }

        public string GetRoomType(int person_id)
        {
            try
            {
                return RoomTypes.GetAll().Join(
                    Agreements.GetAll(),
                    r => r.id_type,
                    a => a.room_id,
                    (r, a) => new KeyValuePair<RoomType, Agreement>(r, a))
                    .Where(res => res.Value.person_id == person_id)
                    .Select(res => res.Key.name).ToList().Last();

                //return (from r in _context.RoomType
                //        join a in _context.Agreement
                //        on r.id_type equals a.room_id
                //        where (a.person_id == person_id)
                //        select r.name).ToList().Last<string>();
            }
            catch { return "Комнатка"; }
        }
        public void GetInformation(
            PersonData person,
            out long? passport,
            out string contract,
            out string roomtype,
            out decimal sum
            )
        {
            int person_id = GetPersonId(person);
            passport = Persons.GetPassport(person_id);
            contract = Persons.GetContract(person_id);
            roomtype = GetRoomType(person_id);
            sum = Accounts.GetSumOnAccount(person_id);
        }

        public void ChangeRoom(PersonData person, NewPlace place, string admin_name, int dorm_id)
        {
            Agreements.GetAll().Where(a => a.person_id == GetPersonId(person)).ToList().
                ForEach(x =>
                {
                    x.floor_number = place.NewFloor;
                    x.room_number = place.NewRoom;
                    x.room_id = place.NewRoomTypeId;
                });

            History.Add(
                new History
                {
                    admin_name = admin_name,
                    dorm_id = dorm_id,
                    description = person.SurName + " " + person.Name + " " + "переехал(-а) из " + person.Room
                + " в " + place.NewRoom,
                    changeRoomFlag = true
                });
        }

        public void MovePersonOut(PersonData person, string admin_name, int dorm_id)
        {
            Persons.GetAll().Where(p => p.id_person == GetPersonId(person)).ToList().ForEach(x => { x.is_living = false; });
            Agreements.GetAll().Where(a => a.person_id == GetPersonId(person)).ToList().ForEach(x => { x.is_active = false; });

            History.Add(new History
            {
                admin_name = admin_name,
                dorm_id = dorm_id,
                description = person.SurName + " " + person.Name + " " + "выселен(-а) из " + person.Room,
                moveOutFlag = true
            });
        }

        public string InsertNewPerson(NewPerson person, string admin_name, int dorm_id)
        {

            _context.InsertPerson(
                person.SurName,
                person.Name,
                person.LastName,
                person.Passport,
                person.IsMale,
                person.IsStudent,
                person.IsBudget,
                Countries.GetCountryId(person.Country),
                person.Contract,
                dorm_id,
                person.Floor,
                person.Room,
                RoomTypes.GetRoomTypeId(person.RoomTypeName),
                person.DateStart,
                person.DateFinish
                );

            string description = "Добавление " + person.SurName + " " + person.Name + " " +
                person.Passport + " " + person.Country + ". Комната:" + " " + person.Room + " " + person.RoomTypeName;

            History.Add(new History
            {
                admin_name = admin_name,
                dorm_id = dorm_id,
                description = description,
                insertFlag = true
            });

            return description;
        }

        public bool Register(RegistrationData registrationData)
        {
            try
            {
                if ((PersonExists(registrationData) == true) &&
                    (Mails.MailAddressReserved(registrationData.EmailAddress) == false) &&
                    (Mails.PersonReserved(GetPersonId(ConvertRegistrationDataToPersonData(registrationData))) == false))
                {
                    Mails.Add(new Mail
                    {
                        person_id = GetPersonId(ConvertRegistrationDataToPersonData(registrationData)),
                        email = registrationData.EmailAddress,
                        password = registrationData.Password,
                    });

                    return true;
                }
                else return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PersonExists(RegistrationData registrationData)
        {
            int roomTypeId, dormId;
            roomTypeId = RoomTypes.GetRoomTypeId(registrationData.RoomType);
            dormId = Dormitories.GetDormIdByName(registrationData.DormitoryName);
            int count = Persons.GetAll().Join(
                                Agreements.GetAll(),
                                p => p.id_person,
                                a => a.person_id,
                                (p, a) => new KeyValuePair<Person, Agreement>(p, a)).Where(
                                x => (x.Key.surname == registrationData.Surname) &&
                                (x.Key.name == registrationData.Name) &&
                                (x.Key.is_living == true) &&
                                (x.Value.room_id == roomTypeId) &&
                                (x.Value.room_number == registrationData.RoomNumber) &&
                                (x.Value.dorm_id == dormId)).Select(res => res.Key.id_person).ToList().Count();
            if (count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public PersonData ConvertRegistrationDataToPersonData(RegistrationData registrationData)
        {
            int roomTypeId, dormId;
            roomTypeId = RoomTypes.GetRoomTypeId(registrationData.RoomType);
            dormId = Dormitories.GetDormIdByName(registrationData.DormitoryName);
            return new PersonData
            {
                Name = registrationData.Name,
                SurName = registrationData.Surname,
                DormID = dormId,
                RoomType = roomTypeId,
                Room = registrationData.RoomNumber
            };
        }

        public string GetRoomGender(int dormId, int roomNumber, int roomTypeId)
        {
            List<bool> isMale;
            isMale = Persons.GetAll()
                .Join(
                Agreements.GetAll(),
                person => person.id_person,
                agreement => agreement.person_id,
                (person, agreement) => new KeyValuePair<Person, Agreement>(person, agreement)
                )
                .Where(res =>
                (res.Key.is_living == true) && (res.Value.room_id == roomTypeId) &&
                (res.Value.is_active == true) && (res.Value.room_number == roomNumber) &&
                (res.Value.dorm_id == dormId))
                .Select(res => res.Key.is_male)
                .ToList();
            if (isMale.Count != 0)
            {
                bool first = isMale.First(); //true, if there is a man in room
                bool q = true; //true, if all the elements the same
                foreach (bool item in isMale)
                {
                    if (item != first)
                    {
                        q = false;
                        break;
                    }
                }

                if (!q) { return "М/Ж"; }
                else if (first) { return "М"; }
                else { return "Ж"; }
            }
            else { return "-"; }
        }

        public PersonData GetPersonDataById(int personId)
        {
            try
            {
                return new PersonData
                {
                    Name = Persons.GetAll().Where(x => x.id_person == personId).Select(x => x.name).LastOrDefault(),
                    SurName = Persons.GetAll().Where(x => x.id_person == personId).Select(x => x.surname).LastOrDefault(),
                    DormID = Agreements.GetAll().Where(x => x.person_id == personId).Select(x => x.dorm_id).LastOrDefault(),
                    Room = Agreements.GetAll().Where(x => x.person_id == personId).Select(x => x.room_number).LastOrDefault(),
                    RoomType = Agreements.GetAll().Where(x => x.person_id == personId).Select(x => x.room_id).LastOrDefault(),
                    Floor = Agreements.GetAll().Where(x => x.person_id == personId).Select(x => x.floor_number).LastOrDefault()
                };
            }
            catch
            {
                Debug.WriteLine("=========== Exception Caught ==== GetPersonDataById");
                return new PersonData { };
            }
        }

        public IEnumerable<int> GetLivingRooms(int floor, int dormId)
        {
            return
                Persons.GetAll()
                .Join(
                Agreements.GetAll(),
                person => person.id_person,
                agreement => agreement.person_id,
                (person, agreement) => new KeyValuePair<Person, Agreement>(person, agreement)
                )
                .Where(res =>
                (res.Key.is_living == true) && (res.Value.floor_number == floor) &&
                (res.Value.is_active == true) && (res.Value.dorm_id == dormId))
                .Select(res => res.Value.room_number)
                .Distinct()
                .ToList();
        }
    }



}