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
    [Authorize] // Restrict access to authenticated users
    public class OrdersController : Controller
    {
        private readonly GarageContext _context; // Database context for interacting with the database

        // Constructor to initialize the context
        public OrdersController(GarageContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index(string search)
        {
            // Start with a queryable collection of orders, including related Car and Customer data
            IQueryable<Order> orders = _context.Order.Include(o => o.Car).ThenInclude(c => c.Customer);

            // If a search query is provided, filter the orders based on customer name, license plate, or chassis number
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower(); // Convert search term to lowercase for case-insensitive comparison
                orders = orders.Where(o =>
                    o.Car.Customer.Name.ToLower().Contains(search) ||
                    o.Car.LicensePlate.ToLower().Contains(search) ||
                    o.Car.ChassisNumber.ToLower().Contains(search)
                );
            }

            return View(await orders.ToListAsync()); // Return the view with the filtered list of orders
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) // Check if the ID is null
            {
                return NotFound(); // Return NotFound result if ID is null
            }

            // Retrieve the order with the specified ID and include related Car and Customer data
            var order = await _context.Order
                .Include(o => o.Car)
                .ThenInclude(c => c.Customer)
                .FirstOrDefaultAsync(m => m.OrderId == id);

            if (order == null) // Check if the order was not found
            {
                return NotFound(); // Return NotFound result if order is null
            }

            return View(order); // Return the view with the order details
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            // Populate the dropdown for selecting a car
            ViewBag.CarID = new SelectList(_context.Car, "CarID", "LicensePlate");
            return View(); // Return the view for creating a new order
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevent Cross-Site Request Forgery (CSRF) attacks
        public async Task<IActionResult> Create([Bind("OrderId,OrderDetails,CarID")] Order order)
        {
            if (ModelState.IsValid) // Check if the model state is valid
            {
                _context.Add(order); // Add the new order to the database
                await _context.SaveChangesAsync(); // Save changes to the database
                return RedirectToAction(nameof(Index)); // Redirect to the Index action
            }
            // Re-populate the dropdown for cars if the model state is invalid
            ViewData["CarID"] = new SelectList(_context.Car, "CarID", "LicensePlate", order.CarID);
            return View(order); // Return the view with the order data
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) // Check if the ID is null
            {
                return NotFound(); // Return NotFound result if ID is null
            }

            var order = await _context.Order.FindAsync(id); // Find the order with the specified ID
            if (order == null) // Check if the order was not found
            {
                return NotFound(); // Return NotFound result if order is null
            }
            // Populate the dropdown for selecting a car
            ViewData["CarID"] = new SelectList(_context.Car, "CarID", "LicensePlate", order.CarID);

            return View(order); // Return the view for editing the order
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevent Cross-Site Request Forgery (CSRF) attacks
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,OrderDetails,CarID")] Order order)
        {
            if (id != order.OrderId) // Check if the ID in the URL matches the ID of the order
            {
                return NotFound(); // Return NotFound result if IDs do not match
            }

            if (ModelState.IsValid) // Check if the model state is valid
            {
                try
                {
                    _context.Update(order); // Update the existing order in the database
                    await _context.SaveChangesAsync(); // Save changes to the database
                    return RedirectToAction(nameof(Index)); // Redirect to the Index action
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId)) // Check for concurrency issues
                    {
                        return NotFound(); // Return NotFound result if order does not exist
                    }
                    else
                    {
                        throw; // Rethrow the exception if it's not a concurrency issue
                    }
                }
            }
            // Re-populate the dropdown for cars if the model state is invalid
            ViewData["CarID"] = new SelectList(_context.Car, "CarID", "LicensePlate", order.CarID);
            return View(order); // Return the view with the order data
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) // Check if the ID is null
            {
                return NotFound(); // Return NotFound result if ID is null
            }

            // Retrieve the order with the specified ID and include related Car data
            var order = await _context.Order
                .Include(o => o.Car)
                .FirstOrDefaultAsync(m => m.OrderId == id);

            if (order == null) // Check if the order was not found
            {
                return NotFound(); // Return NotFound result if order is null
            }

            return View(order); // Return the view for deleting the order
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] // Prevent Cross-Site Request Forgery (CSRF) attacks
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Order == null) // Check if the Order set in the context is null
            {
                return Problem("Entity set 'Garage2Context.Order' is null."); // Return a problem result if context is null
            }
            var order = await _context.Order.FindAsync(id); // Find the order with the specified ID
            if (order != null) // Check if the order was found
            {
                _context.Order.Remove(order); // Remove the order from the database
                await _context.SaveChangesAsync(); // Save changes to the database
            }

            return RedirectToAction(nameof(Index)); // Redirect to the Index action
        }

        // Check if an order with the specified ID exists
        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id); // Return true if order exists, otherwise false
        }
    }
}
