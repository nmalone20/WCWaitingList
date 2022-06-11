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
    [Authorize(Policy = "tutorAndHigherPolicy")]
    public class TutorController : Controller
    {
        private readonly WCWaitingListProjectContext _context;

        public TutorController(WCWaitingListProjectContext context)
        {
            _context = context;
        }

        // GET Tutor
        public async Task<IActionResult> Index()
        {
            var appointments = from s in _context.Appointment
                select s;
            return View(await appointments.Where(s => s.AppointmentMade.Equals(false)).Intersect
                (appointments.Where(s => s.RequestDate.Date.Equals(DateTime.Today))).ToListAsync());
        }
        // GET Tutor/Details/
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

        // GET Tutor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST Tutor/Create
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

        // GET Tutor/Edit/
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

        // POST Tutor/Edit/
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

        // GET Tutor/Delete/
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

        // POST Tutor/Delete/
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