using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DormApp.Domain
{
    public class RegistrationData
    {
        [Required(ErrorMessage = "Введите ваше имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите вашу фамилию")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Введите e-mail")]
        [RegularExpression(@".+\@.+\..+", ErrorMessage = "Введите корректный e-mail")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Выберите корпус")]
        public string DormitoryName { get; set; }

        [Required(ErrorMessage = "Выберите тип комнаты")]
        public string RoomType { get; set; }
        
        [Required(ErrorMessage = "Введите №комнаты")]
        //[RegularExpression(@"[0-9]", ErrorMessage = "Номер комнаты может содержать только цифры")]
        public int RoomNumber { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }

    }
}
