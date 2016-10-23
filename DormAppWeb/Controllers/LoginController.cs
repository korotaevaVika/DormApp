using DormApp.Domain;
using DormApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DormAppWeb.Models;
using System.Diagnostics;

namespace DormAppWeb.Controllers
{
    public class LoginController : Controller
    {
        Dictionary<string, string> _personDataPropertiesAndNames = new Dictionary<string, string>();
        public Dictionary<string, string> DictPersonDataPropertiesAndNames
        {
            get { return _personDataPropertiesAndNames; }
        }

        // GET: Login
        public ActionResult Index()
        {
            if ((Session["PersonId"] != null) && (int.Parse(Session["PersonId"].ToString()) != 0))
            {
                return Exit();
            }
            else { return View(); }
        }

        public ActionResult Exit()
        {
            Session["PersonId"] = null;
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Profile()
        {
            if ((Session["PersonId"] != null) && (int.Parse(Session["PersonId"].ToString()) != 0))
            {
                AccountData accountData;
                PersonData personData;
                using (var unitOfWork = new UnitOfWork(new Dormitory_Entities()))
                {
                    accountData = unitOfWork.GetSuitableTariff(int.Parse(Session["PersonId"].ToString()), int.Parse(Session["DormId"].ToString()));
                    personData = unitOfWork.GetPersonDataById(int.Parse(Session["PersonId"].ToString()));
                    unitOfWork.Dispose();
                }
                ProfileData profileData = new ProfileData
                {
                    accountData = accountData,
                    personData = personData
                };
                FillDictionary();
                ViewData["PersonDataPropertiesAndNames"] = DictPersonDataPropertiesAndNames;
                return View("Profile", profileData);
            }
            else
            {
                return RedirectToAction("Index");
            }
            //return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Index(MailData mailData)
        {
            if (ModelState.IsValid)
            {
                bool? q;
                AccountData accountData;
                PersonData personData;
                int personId, dormId;

                FillDictionary();
                try
                {
                    using (var unitOfWork = new UnitOfWork(new Dormitory_Entities()))
                    {
                        q = unitOfWork.Mails.PasswordCorrect(mailData.EmailAddress, mailData.Password);
                        unitOfWork.GetPersonAndDormIdByEmail(mailData.EmailAddress, out personId, out dormId);
                        accountData = unitOfWork.GetSuitableTariff(personId, dormId);
                        personData = unitOfWork.GetPersonDataById(personId);
                        unitOfWork.Dispose();
                    }
                    if (q.HasValue)
                    {
                        if (q.Value == true)
                        {
                            ProfileData profileData = new ProfileData
                            {
                                accountData = accountData,
                                personData = personData
                            };
                            ViewData["PersonDataPropertiesAndNames"] = DictPersonDataPropertiesAndNames;
                            Session["PersonId"] = personId;
                            Session["DormId"] = dormId;

                            return View("Profile", profileData);
                        }
                        else
                        {
                            ViewBag.Header = "Ошибка при входе";
                            ViewBag.Message = "Неверный пароль";
                            return View("~/Views/Shared/Error.cshtml");
                        }
                    }
                    else
                    {
                        ViewBag.Header = "Ошибка при входе";
                        ViewBag.Message = "Почтовый адрес не зарегистрирован";
                        return View("~/Views/Shared/Error.cshtml");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("\n\n -------------- " + ex.ToString());
                    ViewBag.Message = "Что-то случилось";
                    ViewBag.Header = "Ошибка при входе";
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
            {
                return View();
            }
        }

        private void FillDictionary()
        {
            _personDataPropertiesAndNames.Add("Name", "Имя");
            _personDataPropertiesAndNames.Add("SurName", "Фамилия");
            _personDataPropertiesAndNames.Add("Room", "Комната");
            _personDataPropertiesAndNames.Add("DormID", "Общежитие");
            _personDataPropertiesAndNames.Add("RoomType", "Тип комнаты");
            _personDataPropertiesAndNames.Add("Floor", "Этаж");
        }
    }
}