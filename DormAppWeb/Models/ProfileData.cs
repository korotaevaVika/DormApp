using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DormApp.Domain;
using System.Reflection;
namespace DormAppWeb.Models
{
    public class ProfileData
    {
        public AccountData accountData { get; set; }
        public PersonData personData { get; set; }
    }
}