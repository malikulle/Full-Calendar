using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentistCalender.Mvc.Models;
using DentistCalender.Mvc.Models.Entitiy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentistCalender.Mvc.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public ProfileController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (await _userManager.IsInRoleAsync(user, "Secretary"))
            {
                var dentists = _userManager.Users.Where(x => x.IsDentist).ToList();

                var model = new SecretaryViewModel()
                {
                    User = user,
                    Dentists = dentists,
                    SelectListItems = dentists.Select(x=> new SelectListItem()
                    {
                        Value = x.Id,
                        Text = x.Name + " " + x.Surname
                    }).ToList()
                };

                return View("Secretary",model);
            }
            else
            {
                var model = new SecretaryViewModel()
                {
                    User = user,
                };

                return View("Dentist",model);
            }
          
        }
        [Authorize(Roles = "Secretary")]
        public IActionResult Secretary()
        {
            return View();
        }
        [Authorize(Roles = "Dentist")]
        public IActionResult Dentist()
        {
            return View();
        }
    }
}
