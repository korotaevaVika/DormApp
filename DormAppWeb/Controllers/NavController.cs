using DormApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DormAppWeb.Controllers
{
    public class NavController : Controller
    {
        // GET: Nav
        public string DormMenu()
        {
            return "Hello from NavController";
        }
        public NavController()
        {

        }
        public PartialViewResult Menu()
        {
            List<DormApp.Entities.DormType> dorms;
            using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
            {
                dorms = unitOfWork.Dormitories.GetAll()
                .ToList();
            }
            return PartialView(dorms);
        }
    }
}