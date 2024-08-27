using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage2.Data;
using NET_FRAMEWORKS_EXAMEN_OPDRACHT.Models;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace NET_FRAMEWORKS_EXAMEN_OPDRACHT.Controllers
{
    [Authorize(Roles = "Admin")] // Restrict access to users with the "Admin" role
    public class InvoicesController : Controller
    {
        private readonly GarageContext _context; // Database context for interacting with the database
        private readonly ILogger<InvoicesController> _logger; // Logger for logging information and errors

        // Constructor to initialize the context and logger
        public InvoicesController(GarageContext context, ILogger<InvoicesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Invoices
        public async Task<IActionResult> Index(string sortOrder)
        {
            // Set up sorting parameters based on the sortOrder string
            ViewData["IssueDateSortParm"] = string.IsNullOrEmpty(sortOrder) ? "issuedate_desc" : "";
            ViewData["TotalAmountSortParm"] = sortOrder == "totalamount" ? "totalamount_desc" : "totalamount";

            var invoices = _context.Invoice.AsQueryable(); // Start with a queryable collection of invoices

            // Apply sorting based on the provided sortOrder
            switch (sortOrder)
            {
                case "issuedate_desc":
                    invoices = invoices.OrderByDescending(i => i.IssueDate);
                    break;
                case "totalamount":
                    invoices = invoices.OrderBy(i => i.TotalAmount);
                    break;
                case "totalamount_desc":
                    invoices = invoices.OrderByDescending(i => i.TotalAmount);
                    break;
                default:
                    invoices = invoices.OrderBy(i => i.IssueDate);
                    break;
            }

            // Include related data for the invoices
            invoices = invoices.Include(i => i.Car).Include(c => c.Car.Customer);

            // Calculate the total amount of all invoices
            decimal totalInvoicesSum = invoices.Sum(i => i.TotalAmount);
            ViewBag.TotalInvoicesSum = totalInvoicesSum;

            return View(await invoices.ToListAsync()); // Return the view with the list of invoices
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) // Check if the ID is null
            {
                return NotFound(); // Return NotFound result if ID is null
            }

            // Retrieve the invoice with the specified ID and include related data
            var invoice = await _context.Invoice
                .Include(i => i.Car)
                .Include(i => i.Car.Customer)
                .FirstOrDefaultAsync(m => m.InvoiceId == id);

            if (invoice == null) // Check if the invoice was not found
            {
                return NotFound(); // Return NotFound result if invoice is null
            }

            return View(invoice); // Return the view with the invoice details
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            // Populate the dropdown for selecting a car
            ViewData["Cars"] = new SelectList(_context.Car, "CarID", "LicensePlate");
            return View(); // Return the view for creating a new invoice
        }

        // POST: Invoices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InvoiceId,CarID,IssueDate,TotalAmount,Details")] Invoice invoice)
        {
            if (ModelState.IsValid) // Check if the model state is valid
            {
                try
                {
                    _context.Add(invoice); // Add the new invoice to the database
                    await _context.SaveChangesAsync(); // Save changes to the database
                    _logger.LogInformation($"Invoice with ID {invoice.InvoiceId} created successfully."); // Log the creation
                    return RedirectToAction(nameof(Index)); // Redirect to the Index action
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating invoice."); // Log any errors
                    ModelState.AddModelError("", "Unable to create invoice. Please try again."); // Add an error message
                }
            }

            // Re-populate the dropdown for cars if the model state is invalid
            var cars = _context.Car.Select(c => new SelectListItem
            {
                Value = c.CarID.ToString(),
                Text = c.LicensePlate
            }).ToList();

            ViewData["Cars"] = new SelectList(cars, "Value", "Text");

            return View(invoice); // Return the view with the invoice data
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) // Check if the ID is null
            {
                return NotFound(); // Return NotFound result if ID is null
            }

            var invoice = await _context.Invoice.FindAsync(id); // Find the invoice with the specified ID
            if (invoice == null) // Check if the invoice was not found
            {
                return NotFound(); // Return NotFound result if invoice is null
            }

            // Format the total amount for editing
            string totalAmountString = new string(invoice.TotalAmount.ToString().Where(c => char.IsDigit(c) || c == '.').ToArray());
            invoice.TotalAmount = decimal.Parse(totalAmountString, CultureInfo.InvariantCulture);

            // Set view data for editing
            ViewData["FormattedInvoiceDate"] = invoice.IssueDate.ToString("yyyy-MM-ddTHH:mm");
            ViewData["CarID"] = new SelectList(_context.Car, "CarID", "LicensePlate", invoice.CarID);

            return View(invoice); // Return the view for editing the invoice
        }

        // POST: Invoices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InvoiceId,CarID,IssueDate,TotalAmount,Details")] Invoice invoice)
        {
            if (id != invoice.InvoiceId) // Check if the ID in the URL matches the ID of the invoice
            {
                return NotFound(); // Return NotFound result if IDs do not match
            }

            if (ModelState.IsValid) // Check if the model state is valid
            {
                try
                {
                    _context.Update(invoice); // Update the existing invoice in the database
                    await _context.SaveChangesAsync(); // Save changes to the database
                    _logger.LogInformation($"Invoice with ID {invoice.InvoiceId} updated successfully."); // Log the update
                    return RedirectToAction(nameof(Index)); // Redirect to the Index action
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!InvoiceExists(invoice.InvoiceId)) // Check for concurrency issues
                    {
                        return NotFound(); // Return NotFound result if invoice does not exist
                    }
                    else
                    {
                        _logger.LogError(ex, "Concurrency error updating invoice."); // Log concurrency errors
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating invoice."); // Log any errors
                    ModelState.AddModelError("", "Unable to update invoice. Please try again."); // Add an error message
                }
            }
            ViewData["CarID"] = new SelectList(_context.Car, "CarID", "LicensePlate", invoice.CarID);

            return View(invoice); // Return the view with the invoice data
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) // Check if the ID is null
            {
                return NotFound(); // Return NotFound result if ID is null
            }

            // Retrieve the invoice with the specified ID and include related data
            var invoice = await _context.Invoice
                .Include(i => i.Car)
                .FirstOrDefaultAsync(m => m.InvoiceId == id);

            if (invoice == null) // Check if the invoice was not found
            {
                return NotFound(); // Return NotFound result if invoice is null
            }

            return View(invoice); // Return the view for deleting the invoice
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoice.FindAsync(id); // Find the invoice with the specified ID
            if (invoice != null) // Check if the invoice was found
            {
                _context.Invoice.Remove(invoice); // Remove the invoice from the database
                await _context.SaveChangesAsync(); // Save changes to the database
                _logger.LogInformation($"Invoice with ID {id} deleted successfully."); // Log the deletion
            }

            return RedirectToAction(nameof(Index)); // Redirect to the Index action
        }

        // Check if an invoice with the specified ID exists
        private bool InvoiceExists(int id)
        {
            return _context.Invoice.Any(e => e.InvoiceId == id); // Return true if invoice exists, otherwise false
        }
    }
}
