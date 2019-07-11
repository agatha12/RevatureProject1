using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BankEntities;
using BankingApp.Models;
using Microsoft.AspNetCore.Http;

namespace BankingApp.Controllers
{
    public class LoansController : Controller
    {
        private readonly BankClientContext _context;

        public LoansController(BankClientContext context)
        {
            _context = context;
        }

        // GET: Loans
        public async Task<IActionResult> Index()
        {
            return View(await _context.Loan.ToListAsync());
        }
        public async Task<IActionResult> MyLoan()
        {


            var id = sessionGetId();

            var loan = await _context.Loan.Where(l => l.CustomerId == id).ToListAsync();


            if (loan.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(loan);
            }


        }

        public IActionResult Payment(int id)
        {


            ViewData["Id"] = id;
            return View();



        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment(int id, int amount)
        {


            try
            {
                Loan loan = new Loan();
                loan = await _context.Loan.FirstOrDefaultAsync(c => c.Id == id);

                var x = 0 - amount;


                if (x < loan.Balance)
                {
                    ViewData["ErrorMessage"] = "You can not pay more than the balance of your loan";
                    return View();
                }
                else
                {


              

                    var newBalance = (loan.Balance + amount);
                    loan.Balance = newBalance;


                    _context.Update(loan);
                    await _context.SaveChangesAsync();
                }

            }
            catch
            {
                ViewData["ErrorMessage"] = "There was a problem with your payment please try again";
                return View();
            }
            return RedirectToAction(nameof(MyLoan));


        }


        // GET: Loans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // GET: Loans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Loans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,type,CustomerId,InterestRate,Balance")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                loan.CustomerId = (int)sessionGetId();
                loan.Balance = (0 - loan.Balance);
                loan.type = "loan";
                loan.InterestRate = 4.5;
                _context.Add(loan);
                await _context.SaveChangesAsync();
                return RedirectToAction("MyLoan", "Loans");
            }
            return View(loan);
        }

        // GET: Loans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loan.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }
            return View(loan);
        }

        // POST: Loans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,type,CustomerId,InterestRate,Balance")] Loan loan)
        {
            if (id != loan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanExists(loan.Id))
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
            return View(loan);
        }

        // GET: Loans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // POST: Loans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loan = await _context.Loan.FindAsync(id);
            _context.Loan.Remove(loan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanExists(int id)
        {
            return _context.Loan.Any(e => e.Id == id);
        }
        public int? sessionGetId()
        {
            var val = HttpContext.Session.GetInt32("Id");
            return val;
        }
    }
}
