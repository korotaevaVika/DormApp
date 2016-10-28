using System.Linq;
using DormApp.Entities;
using DormApp.Domain.Interfaces;

namespace DormApp.Domain
{
    class PersonRepository : Repository<Person>, IPersonRepository
    {
        public PersonRepository(Dormitory_Entities context) : base(context)
        {
        }

        public string GetContract(int person_id)
        {
            return GetAll().Where(p => p.id_person == person_id && (p.is_living == true)).Select(p => p.contract).LastOrDefault();
        }

        public long? GetPassport(int person_id)
        {
            return GetAll().Where(p => (p.id_person == person_id) && (p.is_living == true))
                .Select(p => p.passport)
                .ToList()
                .LastOrDefault();
        }
    }
}
