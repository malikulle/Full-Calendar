using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentistCalender.Mvc.Models;
using DentistCalender.Mvc.Models.Entitiy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentistCalender.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            AppUser user = new AppUser()
            {
                UserName = model.Username,
                Name = model.Name,
                Surname = model.Surname,
                Email =model.Email,
                Color = model.Color,
                IsDentist = model.IsDentist
            };

            var result = _userManager.CreateAsync(user, model.Password).Result;

            if (result.Succeeded)
            {
                bool roleCheck = model.IsDentist ?  await AddRole("Dentist") : await AddRole("Secretary");

                if (!roleCheck)
                {
                    return View();
                }

                await _userManager.AddToRoleAsync(user, model.IsDentist ? "Dentist" : "Secretary");
                return RedirectToAction("Index","Home");
            }

            if (result.Errors.Any())
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(err.Code,err.Description);
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("","Kullanıcı Bulunamadı.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Giriş Başarısız");

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Denied()
        {
            return View();
        }
        private async Task<bool> AddRole(string RoleName)
        {
            var roles = _roleManager.Roles.ToList();
            if (!roles.Any(x=>x.Name ==RoleName))
            {
                AppRole role = new AppRole()
                {
                    Name =RoleName
                };

                var result = await _roleManager.CreateAsync(role);

                return result.Succeeded;
            }

            return true;
        } 
    }
}
