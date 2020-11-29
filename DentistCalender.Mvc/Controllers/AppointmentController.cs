using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentistCalender.Mvc.Data;
using DentistCalender.Mvc.Models;
using DentistCalender.Mvc.Models.Entitiy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentistCalender.Mvc.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public JsonResult GetAppointments(string userId)
        {
            var model = _context.Appointments
                //.Where(x=> x.UserId == userId)
                .Include(x => x.User)
                .Select(x => new AppointmentViewModel
                {
                    Id = x.Id,
                    Dentist = x.User.Name + " " + x.User.Surname,
                    PatientName = x.PatientName,
                    PatientSurname = x.PatientSurname,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Description = x.Description,
                    UserId = x.UserId,
                    Color = x.User.Color
                }).AsQueryable();


            if (!string.IsNullOrEmpty(userId))
            {
                model = model.Where(x => x.UserId == userId);
            }


            return Json(model.ToList());
        }

        [HttpPost]
        public JsonResult AddOrUpdateAppointment(AddOrUpdateViewModel model)
        {
            if (model.Id == 0)
            {
                Appointment entity = new Appointment()
                {
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    PatientName =model.PatientName,
                    PatientSurname = model.PatientSurname,
                    Description =model.Description,
                    UserId =  model.UserId
                };

                _context.Add(entity);
            }
            else
            {
                var entity = _context.Appointments.SingleOrDefault(x => x.Id == model.Id);


                if (entity != null)
                {
                    entity.UpdatedDate = DateTime.Now;
                    entity.Description = model.Description;
                    entity.PatientSurname = model.PatientSurname;
                    entity.PatientName = model.PatientName;
                    entity.StartDate = model.StartDate;
                    entity.EndDate = model.EndDate;
                    entity.UserId = model.UserId;
                    _context.Update(entity);
                }
                else
                {
                    return Json("Güncellenecek Veri Bulunamadı");
                }
            }

            _context.SaveChanges();

            return Json("200");
        }

        [HttpPost]
        public JsonResult DeleteAppointment(int Id)
        {
            var entity = _context.Appointments.SingleOrDefault(x => x.Id == Id);

            if (entity==null)
            {
                return Json("Silinecek Veri Bulunamadı");
            }

            _context.Appointments.Remove(entity);
            _context.SaveChanges();

            return Json("200");
        }
    }
}
