using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace DormAppWeb.Classes
{
    public class MailClient
    {
        //after registration passed, send the letter to a person
        public static bool SendConfirmLetter(string email_to, string userName, string userPassword)
        {
            string fromEmail = "korotaevavika97@gmail.com";
            string nameSender = "Отправитель";
            string password = "ctdthyfz108";

            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromEmail, nameSender);
                mail.To.Add(new MailAddress(email_to));
                mail.Subject = "Подтверждение регистрации";
                mail.Body = "<i><h2>Здравствуй, " + userName + "!</h2></i>" +
                    "<p>Регистрация прошла успешно</p>" +
                    "<p>Ваш пароль от личного кабинета: " + userPassword + "</p>" +
                    "<p>С уважением, <br/> Ваш помощник</p>";
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                NetworkCredential credentials = new NetworkCredential(
                    fromEmail,
                    password
                    );
                smtp.Credentials = credentials;

                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Фигня...\n" + ex.GetType().ToString() +
                    "\n" + ex.ToString() + "\n" +
                    ex.StackTrace);
                return false;
            }
        }
    }
}