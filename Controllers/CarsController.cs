using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage2.Data;
using NET_FRAMEWORKS_EXAMEN_OPDRACHT.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
namespace NET_FRAMEWORKS_EXAMEN_OPDRACHT.Controllers






{
    [Authorize] // Ensures that only authenticated users can access the actions in this controller
    public class CarsController : Controller
    {
        private readonly GarageContext _context;

        // Dependency injection to provide the database context
        public CarsController(GarageContext context)
        {
            _context = context;
        }

        // GET: Cars
        // Action to display a list of cars with optional search and sorting functionality
        public async Task<IActionResult> Index(string search, string sortOrder)
        {
            var cars = _context.Car.Include(c => c.Customer).AsQueryable();

            // Set sorting parameters
            ViewData["MakeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "make_desc" : "";
            ViewData["ModelSortParm"] = sortOrder == "model" ? "model_desc" : "model";
            ViewData["LicensePlateSortParm"] = sortOrder == "licenseplate" ? "licenseplate_desc" : "licenseplate";

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                cars = cars
                    .Where(c =>
                        c.LicensePlate.ToLower().Contains(search) ||
                        c.ChassisNumber.ToLower().Contains(search) ||
                        c.Make.ToLower().Contains(search) ||
                        c.Model.ToLower().Contains(search) ||
                        c.Customer.Name.ToLower().Contains(search));
            }

            // Apply sorting based on the selected order
            cars = cars.OrderBy(c => c.Make).ThenBy(c => c.Model).ThenBy(c => c.LicensePlate);

            // Handle descending order sorting
            if (sortOrder == "make_desc")
            {
                cars = cars.OrderByDescending(c => c.Make);
            }
            else if (sortOrder == "model")
            {
                cars = cars.OrderBy(c => c.Model);
            }
            else if (sortOrder == "model_desc")
            {
                cars = cars.OrderByDescending(c => c.Model);
            }
            else if (sortOrder == "licenseplate")
            {
                cars = cars.OrderBy(c => c.LicensePlate);
            }
            else if (sortOrder == "licenseplate_desc")
            {
                cars = cars.OrderByDescending(c => c.LicensePlate);
            }

            return View(await cars.ToListAsync());
        }

        // GET: Cars/Details/5
        // Action to display details of a specific car
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Car == null)
            {
                return NotFound(); // Return 404 if car ID is not provided
            }

            // Fetch car details including the related customer
            var car = await _context.Car
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.CarID == id);
            if (car == null)
            {
                return NotFound(); // Return 404 if car is not found
            }

            return View(car);
        }

        // GET: Cars/Create
        // Action to show the form for creating a new car
        public IActionResult Create()
        {
            // Populate ViewBag with customer list for the dropdown
            ViewBag.CustomerList = new SelectList(_context.Customer, "CustomerId", "Name");
            return View();
        }

        // POST: Cars/Create
        // Action to handle the form submission for creating a new car
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarID,CustomerId,Make,Model,LicensePlate,ChassisNumber")] Car car)
        {
            if (ModelState.IsValid) // Check if the model is valid
            {
                car.Customer = _context.Customer.FirstOrDefault(c => c.CustomerId == car.CustomerId);

                _context.Add(car);
                await _context.SaveChangesAsync(); // Save the new car to the database
                return RedirectToAction(nameof(Index)); // Redirect to the car list
            }

            // Log validation errors if the model state is invalid
            Console.WriteLine("Validation errors in the model:");
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }
            }

            // Repopulate the customer list and return to the view with validation errors
            ViewBag.CustomerList = new SelectList(_context.Customer, "CustomerId", "Name", car.CustomerId);
            return View(car);
        }

        // GET: Cars/Edit/5
        // Action to show the form for editing an existing car
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Car == null)
            {
                return NotFound(); // Return 404 if car ID is not provided
            }

            // Fetch the car to edit
            var car = await _context.Car.FindAsync(id);
            if (car == null)
            {
                return NotFound(); // Return 404 if car is not found
            }

            // Populate ViewBag with customer list for the dropdown
            ViewBag.CustomerList = new SelectList(_context.Customer, "CustomerId", "Name", car.CustomerId);

            return View(car);
        }

        // POST: Cars/Edit/5
        // Action to handle the form submission for editing an existing car
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarID,CustomerId,Make,Model,LicensePlate,ChassisNumber")] Car car)
        {
            if (id != car.CarID) // Check if the provided ID matches the car's ID
            {
                return NotFound(); // Return 404 if IDs do not match
            }

            if (ModelState.IsValid) // Check if the model is valid
            {
                car.Customer = _context.Customer.FirstOrDefault(c => c.CustomerId == car.CustomerId);

                try
                {
                    _context.Update(car); // Update the car in the database
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) // Handle concurrency issues
                {
                    if (!CarExists(car.CarID))
                    {
                        return NotFound(); // Return 404 if car no longer exists
                    }
                    else
                    {
                        throw; // Re-throw the exception if it's an unknown issue
                    }
                }
                return RedirectToAction(nameof(Index)); // Redirect to the car list
            }

            // Repopulate the customer list and return to the view with validation errors
            ViewBag.CustomerList = new SelectList(_context.Customer, "CustomerId", "Name", car.CustomerId);

            return View(car);
        }

        // GET: Cars/Delete/5
        // Action to show the confirmation page for deleting a car
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Car == null)
            {
                return NotFound(); // Return 404 if car ID is not provided
            }

            // Fetch the car to delete
            var car = await _context.Car
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.CarID == id);
            if (car == null)
            {
                return NotFound(); // Return 404 if car is not found
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        // Action to handle the deletion of a car after confirmation
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Fetch the car and its associated orders
                var car = await _context.Car
                    .Include(c => c.Orders)
                    .FirstOrDefaultAsync(c => c.CarID == id);

                if (car == null)
                {
                    return NotFound(); // Return 404 if car is not found
                }

                // Remove associated orders and then the car itself
                _context.Order.RemoveRange(car.Orders);
                _context.Car.Remove(car);

                await _context.SaveChangesAsync(); // Save changes to the database

                return RedirectToAction(nameof(Index)); // Redirect to the car list
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException && sqlException.Number == 547)
                {
                    // Handle foreign key violation when attempting to delete a car with associated orders/invoices
                    ViewData["ErrorMessage"] = "You cannot delete this car because there are orders or invoices for this car.";
                    return View("Error");
                }

                Console.WriteLine(ex); // Log the exception details
                throw; // Re-throw the exception for further handling
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // Log the exception details
                throw; // Re-throw the exception for further handling
            }
        }

        // Helper method to check if a car exists in the database
        private bool CarExists(int id)
        {
            return (_context.Car?.Any(e => e.CarID == id)).GetValueOrDefault();
        }
    }
}
