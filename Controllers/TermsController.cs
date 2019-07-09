using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BankEntities;
using BankingApp.Models;

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
                _context.Add(term);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
    }
}
