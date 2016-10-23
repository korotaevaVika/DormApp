using DormApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DormAppWeb.Models
{
    public class DormListViewModel
    {
        public List<DormType> Dormitories { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}