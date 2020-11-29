using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DentistCalender.Mvc.Models.Entitiy
{
    public class AppUser : IdentityUser
    {
        public bool IsDentist { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public string Color { get; set; }

        public List<Appointment> Appointments { get; set; }
    }
}
