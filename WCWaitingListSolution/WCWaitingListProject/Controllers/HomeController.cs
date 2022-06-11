#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WCWaitingListProject.Models;
using WCWaitingListSolution.Data;
using Microsoft.AspNetCore.Authorization;

namespace WCWaitingListProject.WCWaitingListProject
{ 
    [Authorize]
    [Authorize(Policy = "adminAndHigherPolicy")]
    public class HomeController : Controller
    {
        private readonly WCWaitingListProjectContext _context;

        public HomeController(WCWaitingListProjectContext context)
        {
            _context = context;
        }

        // GET Home
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, string FilterAppointmentStatus)
        {
            var appointments = from s in _context.Appointment
                select s;
            
            // If there is just a start date, returns everything after that date
            if (!String.IsNullOrEmpty(startDate.ToString()) && String.IsNullOrEmpty(endDate.ToString()))
            {
                if (FilterAppointmentStatus.Contains("request")){
                    appointments = appointments.Where(s => startDate <= (s.RequestDate.Date)).Intersect(
                        appointments.Where( s => s.AppointmentMade.Equals(false)).OrderByDescending(s => s.RequestDate));
                }
                else if (FilterAppointmentStatus.Contains("made")){
                    appointments = appointments.Where(s => startDate <= (s.RequestDate.Date)).Intersect(
                        appointments.Where( s => s.AppointmentMade == true).OrderByDescending(s => s.RequestDate));
                }
                else if (FilterAppointmentStatus.Contains("all")){
                    appointments = appointments.Where(s => startDate <= (s.RequestDate.Date)).OrderByDescending(s => s.RequestDate);
                }
            }

            // If there just an end date, returns everything before that date
            else if (String.IsNullOrEmpty(startDate.ToString()) && !String.IsNullOrEmpty(endDate.ToString()))
            {
                if (FilterAppointmentStatus.Contains("request")){
                    appointments = appointments.Where(s => endDate >= (s.RequestDate.Date)).Intersect(
                        appointments.Where( s => s.AppointmentMade.Equals(false)).OrderByDescending(s => s.RequestDate));
                }
                else if (FilterAppointmentStatus.Contains("made")){
                    appointments = appointments.Where(s => endDate >= (s.RequestDate.Date)).Intersect(
                        appointments.Where( s => s.AppointmentMade == true).OrderByDescending(s => s.RequestDate));
                }
                else if (FilterAppointmentStatus.Contains("all")){
                    appointments = appointments.Where(s => endDate >= (s.RequestDate.Date)).OrderByDescending(s => s.RequestDate);
                }
            }

            // If both a start and end date are given 
            else if (!String.IsNullOrEmpty(startDate.ToString()) && !String.IsNullOrEmpty(endDate.ToString()))
            {
                if (FilterAppointmentStatus.Contains("request")){
                    appointments = appointments.Where(s => endDate >= (s.RequestDate.Date)).Intersect(
                        appointments.Where(s => startDate <= (s.RequestDate.Date))).Intersect(
                        appointments.Where( s => s.AppointmentMade.Equals(false)).OrderByDescending(s => s.RequestDate));
                }
                else if (FilterAppointmentStatus.Contains("made")){
                    appointments = appointments.Where(s => endDate >= (s.RequestDate.Date)).Intersect(
                        appointments.Where(s => startDate <= (s.RequestDate.Date))).Intersect(
                        appointments.Where( s => s.AppointmentMade == true).OrderByDescending(s => s.RequestDate));
                }
                else if (FilterAppointmentStatus.Contains("all")){
                    appointments = appointments.Where(s => endDate >= (s.RequestDate.Date)).Intersect(
                    appointments.Where(s => startDate <= (s.RequestDate.Date))).OrderByDescending(s => s.RequestDate);
                }
            }

            // If there is no date given
            else if (String.IsNullOrEmpty(startDate.ToString()) && String.IsNullOrEmpty(endDate.ToString())) {
                if (String.IsNullOrEmpty(FilterAppointmentStatus)){
                    appointments = appointments.OrderByDescending(s => s.RequestDate);
                }
                else if (FilterAppointmentStatus.Contains("all")){
                    appointments = appointments.OrderByDescending(s => s.RequestDate);
                }
                else if (FilterAppointmentStatus.Contains("request")){
                    appointments = appointments.Where( s => s.AppointmentMade == false).OrderByDescending(s => s.RequestDate);
                }
                else if (FilterAppointmentStatus.Contains("made")){
                    appointments = appointments.Where( s => s.AppointmentMade.Equals(true)).OrderByDescending(s => s.RequestDate);
                }
            }
            return View(await appointments.OrderByDescending(s => s.RequestDate).ToListAsync());
        }

        // GET Home/Details/
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET Home/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST Home/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,RequestDate,Limits,AppointmentMade,Notes")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appointment);
        }

        // GET Home/Edit/
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return View(appointment);
        }

        // POST Home/Edit/
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,RequestDate,Limits,AppointmentMade,Notes")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(appointment);
        }

        // GET Home/Delete/
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST Home/Delete/
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointment.FindAsync(id);
            _context.Appointment.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointment.Any(e => e.Id == id);
        }
    }
}
