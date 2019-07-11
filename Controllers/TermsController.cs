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
    public class TermsController : Controller
    {
        private readonly BankClientContext _context;

        public TermsController(BankClientContext context)
        {
            _context = context;
        }

        // GET: Terms
        public async Task<IActionResult> Index()
        {
            return View(await _context.Term.ToListAsync());
        }

        public async Task<IActionResult> MyTerm()
        {


            var id = sessionGetId();

            var term = await _context.Term.Where(t => t.CustomerId == id).ToListAsync();


            if (term.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(term);
            }


        }

        public IActionResult Withdraw(int id)
        {

            ViewData["Id"] = id;
            return View();



        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdraw(int id, int amount)
        {


            try
            {
                Term term = new Term();
                term = await _context.Term.FirstOrDefaultAsync(c => c.Id == id);
                int difference = DateTime.Compare(DateTime.Now, term.createdAt.AddMonths(6));

                if (difference <= 0)
                {
                    
                        ViewData["ErrorMessage"] = $"You can not withdraw from this account until {term.createdAt.AddMonths(6)}";
                    return View();
                }

                else if (term.Balance < amount)
                {
                    ViewData["ErrorMessage"] = $"You tried to withdraw ${amount} but your balance is only ${term.Balance}";
                    return View();
                }
                else
                {
                    var newBalance = (term.Balance - amount);
                    term.Balance = newBalance;


                    _context.Update(term);
                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
                ViewData["ErrorMessage"] = "There was a problem with your withdrawl please try again";
                return View();
            }
            return RedirectToAction(nameof(MyTerm));


        }

        public IActionResult Deposit(int id)
        {


            ViewData["Id"] = id;
            return View();



        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit(int id, int amount)
        {


            try
            {
                Term term = new Term();
                term = await _context.Term.FirstOrDefaultAsync(c => c.Id == id);


                var newBalance = (term.Balance + amount);
                term.Balance = newBalance;


                _context.Update(term);
                await _context.SaveChangesAsync();

            }
            catch
            {
                ViewData["ErrorMessage"] = "There was a problem with your deposit please try again";
                return View();
            }
            return RedirectToAction(nameof(MyTerm));


        }

        // GET: Terms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var term = await _context.Term
                .FirstOrDefaultAsync(m => m.Id == id);
            if (term == null)
            {
                return NotFound();
            }

            return View(term);
        }

        // GET: Terms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Terms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,type,CustomerId,InterestRate,Balance,createdAt")] Term term)
        {
            if (ModelState.IsValid)
            {
                term.CustomerId = (int)sessionGetId();
                term.InterestRate = 3;
                term.type = "term";
                term.createdAt = DateTime.Now;
                _context.Add(term);
                await _context.SaveChangesAsync();
                return RedirectToAction("MyTerm", "Terms");
            }
            return View(term);
        }

        // GET: Terms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var term = await _context.Term.FindAsync(id);
            if (term == null)
            {
                return NotFound();
            }
            return View(term);
        }

        // POST: Terms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,type,CustomerId,InterestRate,Balance,createdAt")] Term term)
        {
            if (id != term.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(term);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TermExists(term.Id))
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
            return View(term);
        }

        // GET: Terms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var term = await _context.Term
                .FirstOrDefaultAsync(m => m.Id == id);
            if (term == null)
            {
                return NotFound();
            }

            return View(term);
        }

        // POST: Terms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var term = await _context.Term.FindAsync(id);
            _context.Term.Remove(term);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TermExists(int id)
        {
            return _context.Term.Any(e => e.Id == id);
        }
        public int? sessionGetId()
        {
            var val = HttpContext.Session.GetInt32("Id");
            return val;
        }
    }
}
