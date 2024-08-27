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
using static NuGet.Packaging.PackagingConstants;
using Microsoft.AspNetCore.Authorization;
namespace NET_FRAMEWORKS_EXAMEN_OPDRACHT.Controllers
{


    //// GET: Customers/Details/5
    //public async Task<IActionResult> Details(int? id)
    //{

    //    if (id == null || _context.Customer == null)
    //    {
    //        return NotFound();
    //    }

    //    var customer = await _context.Customer
    //        .Include(c => c.Cars)
    //        .FirstOrDefaultAsync(m => m.CustomerId == id);
    //    if (customer == null)
    //    {
    //        return NotFound();
    //    }

    //    return View(customer);
    //}



    [Authorize]
    public class CustomersController : Controller
    {
        private readonly GarageContext _context;

        public CustomersController(GarageContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index(string search, string sortOrder)
        {
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            var customers = await _context.Customer.ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                customers = customers.Where(c =>
                    c.Name.ToLower().Contains(search) ||
                    c.Email.ToLower().Contains(search) ||
                    c.Adress.ToLower().Contains(search) ||
                    c.PhoneNumber.ToLower().Contains(search)
                ).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    customers = customers.OrderByDescending(c => c.Name).ToList();
                    break;
                default:
                    customers = customers.OrderBy(c => c.Name).ToList();
                    break;
            }

            return View(customers);
        }



        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null || _context.Customer == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .Include(c => c.Cars)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,Name,Email,Adress,PhoneNumber")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //voy a probar aver si salen fallos porque no me deja crear un customer
            Console.WriteLine("Errores de validación en el modelo:");
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }
            }
            // lo de arriba
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Customer == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,Name,Email,Adress,PhoneNumber")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Customer == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var customer = await _context.Customer.FindAsync(id);

                if (customer == null)
                {
                    return Problem("El cliente no se encontró.");
                }

                _context.Customer.Remove(customer);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException && sqlException.Number == 547)
                {
                    ViewData["ErrorMessage"] = "You cannot delete this customer because there are cars on his/her name.";
                    return View("Error");
                }

                Console.WriteLine(ex);
                throw;
            }
        }




        private bool CustomerExists(int id)
        {
          return (_context.Customer?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}
