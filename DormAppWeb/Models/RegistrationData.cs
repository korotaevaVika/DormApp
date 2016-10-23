using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DormApp.Entities;
using DormApp.Domain;


namespace DormAppWeb.Models
{
    public class RegistrationData
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public string DormitoryName { get; set; }
        public string RoomType { get; set; }
        public int RoomNumber { get; set; }
        public string Password { get; set; }

        private string Convert(RegistrationData regData)
        {
            int roomTypeId, dormId;
            string message = string.Empty;
            try
            {
                using (var unitOfWork = new UnitOfWork(new Dormitory_Entities()))
                {
                    roomTypeId = unitOfWork.RoomTypes.GetRoomTypeId(RoomType);
                    dormId = unitOfWork.Dormitories.GetDormIdByName(DormitoryName);

                    unitOfWork.Dispose();
                }
                message = "Регистрация прошла успешно";
            }
            catch (Exception ex)
            {
                message = "Ошибка во время регистрации\n" + ex.ToString();

            }
            return message;
        }
    }
}