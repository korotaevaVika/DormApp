using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DormApp.Entities;
namespace DormApp.Domain.Interfaces
{
    public interface IMailRepository : IRepository<Mail>
    {
        bool MailAddressReserved(string EmailAddress);
        bool PersonReserved(int personId);
        bool MailExisted(string EmailAddress);
        bool? PasswordCorrect(string EmailAddress, string password);
    }
}
