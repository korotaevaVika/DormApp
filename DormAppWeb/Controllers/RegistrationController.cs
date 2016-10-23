using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DormApp.Domain;
using DormApp.Entities;
using System.Data.SqlClient;
using System.Diagnostics;
using DormAppWeb.Classes;

namespace DormAppWeb.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        public ActionResult Index()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new Dormitory_Entities()))
                {
                    ViewBag.DormitoryList = unitOfWork.Dormitories.GetDormNames().ToList();
                    ViewBag.RoomtypesList = unitOfWork.RoomTypes.GetRoomTypeNames().ToList();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\n\n------------\n" + ex.ToString());
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(RegistrationData registrationData)
        {
            using (var unitOfWork = new UnitOfWork(new Dormitory_Entities()))
            {
                ViewBag.DormitoryList = unitOfWork.Dormitories.GetDormNames().ToList();
                ViewBag.RoomtypesList = unitOfWork.RoomTypes.GetRoomTypeNames().ToList();
                unitOfWork.Dispose();
            }

            if (ModelState.IsValid)
            {
                bool q, mailAddressReserved, personExists;
                mailAddressReserved = false;
                personExists = true;
                try
                {
                    using (var unitOfWork = new UnitOfWork(new Dormitory_Entities()))
                    {
                        q = unitOfWork.Register(registrationData);

                        if (!q)
                        {
                            mailAddressReserved = unitOfWork.Mails.MailAddressReserved(registrationData.EmailAddress);
                            personExists = unitOfWork.PersonExists(registrationData);
                        }
                        personExists = unitOfWork.PersonExists(registrationData);
                        unitOfWork.Complete();
                        unitOfWork.Dispose();
                    }
                    if (q)
                    {
                        if (MailClient.SendConfirmLetter(
                            registrationData.EmailAddress,
                            registrationData.Name,
                            registrationData.Password))
                        {
                            return View("Confirm", registrationData);
                        }
                        else
                        {
                            ViewBag.Header = "Ошибка при посылке почтового письма";
                            ViewBag.Message = "Тем не менее вы зарегистрированы";
                            return View("~/Views/Shared/Error.cshtml");
                        }


                    }
                    else if (mailAddressReserved)
                    {
                        ViewBag.Header = "Ошибка при регистрации";
                        ViewBag.Message = "Данный почтовый ящик уже зарегистрирован";
                        return View("~/Views/Shared/Error.cshtml");
                    }
                    else if (!personExists)
                    {
                        ViewBag.Header = "Ошибка при регистрации";
                        ViewBag.Message = "Не найдено совпадения в базе проживающих";
                        return View("~/Views/Shared/Error.cshtml");
                    }
                    else
                    {
                        ViewBag.Header = "Ошибка при регистрации";
                        ViewBag.Message = "Данный человек уже зарегистрирован";
                        return View("~/Views/Shared/Error.cshtml");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("\n\n -------------- " + ex.ToString());
                    ViewBag.Message = "Во время регистрации произошла ошибка";
                    ViewBag.Header = "Ошибка при регистрации";
                    return View("~/Views/Shared/Error.cshtml");
                }

            }
            else
            {
                return View();
            }

        }
    }
}