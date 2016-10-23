using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DormApp.Domain
{
    public class MailData
    {
        [Required(ErrorMessage = "Введите e-mail")]
        [RegularExpression(@".+\@.+\..+", ErrorMessage = "Введите корректный e-mail")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }
    }
}
