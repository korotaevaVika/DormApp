using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Configuration;
//using DormAppWeb.Models;
using System.Diagnostics;

namespace DormAppWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //ViewBag.LinkToMisis = WebConfigurationManager.AppSettings["LinkToMisis"];
            return View();
        }
        
    }
}