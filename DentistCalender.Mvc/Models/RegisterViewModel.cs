using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentistCalender.Mvc.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Kullanıcı Adı Zorunlu")]
        [Display(Name = "Kullanıcı Adı")]

        public string Username { get; set; }
        [Required(ErrorMessage = "İsim Zorunlu")]
        [Display(Name = "İsim")]

        public string Name { get; set; }
        [Required(ErrorMessage = "Soyisim Zorunlu")]
        [Display(Name = "Soyisim")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Şifre Zorunlu")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email Zorunlu")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Renk")]
        public string Color { get; set; }

        [Display(Name = "Diş Hekimisiniz ?")]
        public bool IsDentist { get; set; }

    }
}
