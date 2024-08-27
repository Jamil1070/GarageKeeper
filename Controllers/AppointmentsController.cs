using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage2.Data;
using NET_FRAMEWORKS_EXAMEN_OPDRACHT.Models;
using Microsoft.AspNetCore.Authorization;

namespace NET_FRAMEWORKS_EXAMEN_OPDRACHT.Controllers
{
    // Ensures that all actions in this controller require the user to be authorized
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly GarageContext _context;

        // Constructor to inject the database context
        public AppointmentsController(GarageContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index(string statusFilter, string search, DateTime? dateFilter)
        {
            // Query to include related Car entities with each appointment
            var appointments = _context.Appointment.Include(a => a.Car).AsQueryable();

            // Filter appointments by status if provided
            if (!string.IsNullOrEmpty(statusFilter))
            {
                appointments = appointments.Where(a => a.Status == statusFilter);
            }

            // Search for appointments based on service or license plate
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                appointments = appointments.Where(a =>
                    a.RequiredService.ToLower().Contains(search) ||
                    a.Car.LicensePlate.ToLower().Contains(search)
                );
            }

            // Filter appointments by the selected date
            if (dateFilter.HasValue)
            {
                appointments = appointments.Where(a => a.AppointmentDate.Date == dateFilter.Value.Date);
            }

            // Order appointments by appointment date
            appointments = appointments.OrderBy(a => a.AppointmentDate);

            // Convert query to list and pass it to the view
            var appointmentList = await appointments.ToListAsync();

            return View(appointmentList);
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();  // Return 404 if ID is null
            }

            // Retrieve appointment details along with related car and customer data
            var appointment = await _context.Appointment
                .Include(a => a.Car)
                    .ThenInclude(c => c.Customer)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);

            if (appointment == null)
            {
                return NotFound();  // Return 404 if the appointment is not found
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            // Populate dropdown list for selecting a car
            ViewBag.Cars = new SelectList(_context.Car, "CarID", "LicensePlate");

            return View();
        }

        // POST: Appointments/Create
        // Bind specific properties to protect from overposting attacks
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentId,CarID,AppointmentDate,RequiredService,Status")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                // Add the new appointment to the database
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));  // Redirect to the index action
            }

            // Repopulate the dropdown list if the model state is invalid
            ViewBag.Cars = new SelectList(_context.Car, "CarID", "LicensePlate");

            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Appointment == null)
            {
                return NotFound();  // Return 404 if ID or context is null
            }

            // Retrieve the appointment to be edited
            var appointment = await _context.Appointment.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();  // Return 404 if the appointment is not found
            }

            // Populate dropdown list for cars and format the appointment date for the input field
            ViewBag.Cars = new SelectList(_context.Car.ToList(), "CarID", "LicensePlate");
            ViewData["FormattedAppointmentDate"] = appointment.AppointmentDate.ToString("yyyy-MM-ddTHH:mm");

            return View(appointment);
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentId,CarID,AppointmentDate,RequiredService,Status")] Appointment appointment)
        {
            if (id != appointment.AppointmentId)
            {
                return NotFound();  // Return 404 if ID mismatch
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update the appointment in the database
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.AppointmentId))
                    {
                        return NotFound();  // Return 404 if the appointment no longer exists
                    }
                    else
                    {
                        throw;  // Re-throw exception if another error occurs
                    }
                }
                return RedirectToAction(nameof(Index));  // Redirect to the index action
            }

            // Repopulate the dropdown list if the model state is invalid
            ViewData["CarID"] = new SelectList(_context.Car, "CarID", "CarID", appointment.CarID);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Appointment == null)
            {
                return NotFound();  // Return 404 if ID or context is null
            }

            // Retrieve the appointment to be deleted along with related car data
            var appointment = await _context.Appointment
                .Include(a => a.Car)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);

            if (appointment == null)
            {
                return NotFound();  // Return 404 if the appointment is not found
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Appointment == null)
            {
                return Problem("Entity set 'Garage2Context.Appointment' is null.");  // Return problem if context is null
            }

            // Find and remove the appointment from the database
            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointment.Remove(appointment);
            }

            await _context.SaveChangesAsync();  // Save changes to the database
            return RedirectToAction(nameof(Index));  // Redirect to the index action
        }

        // Check if an appointment exists in the database by its ID
        private bool AppointmentExists(int id)
        {
            return (_context.Appointment?.Any(e => e.AppointmentId == id)).GetValueOrDefault();
        }
    }
}
