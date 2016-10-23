using DormApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DormAppWeb.Models
{
    public class TariffListViewModel
    {
        public List<Tariff> Tariffs { get; set; }
        public PageInfo PageInfo { get; set; }
        public int CurrentDormId { get; set; }
        public string CurrentTranslitDormName { get; set; }
    }
}