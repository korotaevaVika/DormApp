using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DormApp.Domain
{
    public class Tariff
    {
        public string Title { get; set; }
        public string RoomTypeName { get; set; }
        public decimal Price { get; set; }
        public bool IsStudent { get; set; }
        public bool OnBudget { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateFinish { get; set; }
        public int DormId { get; set; }
    }
}
