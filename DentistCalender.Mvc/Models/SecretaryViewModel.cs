using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentistCalender.Mvc.Models.Entitiy;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentistCalender.Mvc.Models
{
    public class SecretaryViewModel
    {
        public AppUser User { get; set; }
        public List<AppUser> Dentists { get; set; }
        public List<SelectListItem> SelectListItems { get; set; }
    }
}
