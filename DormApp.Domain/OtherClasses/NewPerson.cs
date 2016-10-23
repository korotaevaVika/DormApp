using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DormApp.Domain
{
    public class NewPerson
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string LastName { get; set; }
        public long Passport { get; set; }
        public bool IsMale { get; set; }
        public bool IsStudent { get; set; }
        public bool IsBudget { get; set; }
        public string Country { get; set; }
        public string Contract { get; set; }
        public int Floor { get; set; }
        public int Room { get; set; }
        public string RoomTypeName { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateFinish { get; set; }
    }
}
