using System.Linq;
using DormApp.Entities;
using DormApp.Domain.Interfaces;

namespace DormApp.Domain
{
    class MailRepository : Repository<Mail>, IMailRepository
    {
        public MailRepository(Dormitory_Entities context) : base(context)
        {
        }

        public bool MailAddressReserved(string EmailAddress)
        {
            int count = GetAll().Where(m => m.email == EmailAddress).Select(m => m).Count();
            if (count == 0) { return false; } else { return true; }
        }

        public bool MailExisted(string EmailAddress)
        {

            return (GetAll().Where(x => x.email.Equals(EmailAddress)).Select(x => x).Count() == 1);
        }

        
        public bool? PasswordCorrect(string EmailAddress, string password)
        {
            if (MailExisted(EmailAddress))
            {
                if (GetAll().Where(
                    x => ((x.email.Equals(EmailAddress) &&
                    (x.password.Equals(password))))).Select(x => x).Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return null;
        }

        public bool PersonReserved(int personId)
        {
            int count = GetAll().Where(m => m.person_id == personId).Select(m => m).Count();
            if (count == 0) { return false; } else { return true; }
        }
    }
}
