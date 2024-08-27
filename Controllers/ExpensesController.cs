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
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Authorization;

namespace NET_FRAMEWORKS_EXAMEN_OPDRACHT.Controllers
{

//    // POST: Expenses/Delete/5
//    [HttpPost, ActionName("Delete")]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> DeleteConfirmed(int id)
//    {
//        if (_context.Expense == null)
//        {
//            return Problem("Entity set 'GarageContext.Expense'  is null.");
//        }
//        var expense = await _context.Expense.FindAsync(id);
//        if (expense != null)
//        {
//            _context.Expense.Remove(expense);
//        }

//        await _context.SaveChangesAsync();
//        return RedirectToAction(nameof(Index));
//    }

//    private bool ExpenseExists(int id)
//    {
//        return (_context.Expense?.Any(e => e.ExpenseId == id)).GetValueOrDefault();
//    }
//}
//}




    [Authorize(Roles = "Admin")]

    public class ExpensesController : Controller
    {
        private readonly GarageContext _context;

        public ExpensesController(GarageContext context)
        {
            _context = context;
        }

        // GET: Expenses
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["DateSortParm"] = string.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["AmountSortParm"] = sortOrder == "amount" ? "amount_desc" : "amount";

            var expenses = _context.Expense.AsQueryable();

            switch (sortOrder)
            {
                case "date_desc":
                    expenses = expenses.OrderByDescending(e => e.Date);
                    break;
                case "amount":
                    expenses = expenses.OrderBy(e => e.Amount);
                    break;
                case "amount_desc":
                    expenses = expenses.OrderByDescending(e => e.Amount);
                    break;
                default:
                    expenses = expenses.OrderBy(e => e.Date);
                    break;
            }

            decimal totalExpensesSum = expenses.Sum(i => i.Amount);
            ViewBag.TotalExpensesSum = totalExpensesSum;


            return View(await expenses.ToListAsync());
        }


        // GET: Expenses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Expense == null)
            {
                return NotFound();
            }

            var expense = await _context.Expense
                .FirstOrDefaultAsync(m => m.ExpenseId == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // GET: Expenses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Expenses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExpenseId,Description,Amount,Date")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expense);
        }

        // GET: Expenses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Expense == null)
            {
                return NotFound();
            }

            var expense = await _context.Expense.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

           

            string amountString = new string(expense.Amount.ToString().Where(c => char.IsDigit(c) || c == '.').ToArray());

            
            expense.Amount = decimal.Parse(amountString, CultureInfo.InvariantCulture) / 100;


            return View(expense);
        }


        // POST: Expenses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExpenseId,Description,Amount,Date")] Expense expense)
        {
            if (id != expense.ExpenseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expense);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(expense.ExpenseId))
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
            return View(expense);
        }

        // GET: Expenses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Expense == null)
            {
                return NotFound();
            }

            var expense = await _context.Expense
                .FirstOrDefaultAsync(m => m.ExpenseId == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Expense == null)
            {
                return Problem("Entity set 'GarageContext.Expense'  is null.");
            }
            var expense = await _context.Expense.FindAsync(id);
            if (expense != null)
            {
                _context.Expense.Remove(expense);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseExists(int id)
        {
          return (_context.Expense?.Any(e => e.ExpenseId == id)).GetValueOrDefault();
        }
    }
}
