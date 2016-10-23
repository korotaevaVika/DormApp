using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace DormAppWeb
{
    public class GlobalSettings
    {
        static public string LinkToMisis { get; set; }

        static public string ViewOfHomePage { get; set; }
        static public string ControllerOfHomePage { get; set; }


        static public string ViewOfRegistrationPage { get; set; }
        static public string NameLinkOfRegistrationPage { get; set; }
        static public string ControllerOfRegistrationPage { get; set; }

        static public string NameLinkOfLoginPage { get; set; }
        static public string ViewOfLoginPage { get; set; }
        static public string ControllerOfLoginPage { get; set; }

        static public string NameLinkOfPersonalPage { get; set; }
        static public string ViewOfPersonalPage { get; set; }
        static public string ControllerOfPersonalPage { get; set; }

        static public string NameLinkOfTariffs { get; set; }
        static public string ViewOfTariffs { get; set; }
        static public string ControllerOfTariffs { get; set; }

        static GlobalSettings()
        {
            LinkToMisis = WebConfigurationManager.AppSettings["LinkToMisis"];

            ViewOfHomePage = "Index";
            ControllerOfHomePage = "Home";

            NameLinkOfRegistrationPage = "Регистрация";
            ViewOfRegistrationPage = "Index";
            ControllerOfRegistrationPage = "Registration";

            NameLinkOfLoginPage = "Войти";
            ViewOfLoginPage = "Index";
            ControllerOfLoginPage = "Login";

            NameLinkOfPersonalPage = "Личный кабинет";
            ViewOfPersonalPage = "Profile";
            ControllerOfPersonalPage = "Login";

            NameLinkOfTariffs = "Тарифы";
            ViewOfTariffs = "Tariffs";
            ControllerOfTariffs = "Info";
    }
    }
}