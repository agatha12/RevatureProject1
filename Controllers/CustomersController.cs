using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BankEntities;
using BankingApp.Models;
using BankingApp.BusinessLayer;

namespace BankingApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerBL _bl;

        public CustomersController(BankClientContext context)
        {
            // _context = context;
            _bl = new CustomerBL(context);
        }

        // GET: Customers
        //public async Task<IActionResult> Index()

        //{
        //return View(await _context.Customers.ToListAsync());
        //    var customers = _context.Get();
        //}
        public IActionResult Index()
        {
            //return View(await _context.Customers.ToListAsync());
            var customers = _bl.Get();
            return View(customers);
        }

        // GET: Customers/Details/5
        //public async Task<IActionResult> Details(int? id)
        //public Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //var customer = await _context.Customers
        //   .FirstOrDefaultAsync(m => m.Id == id);
        //var customer = _context.Get(id);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(customer);
        //}
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var customer = await _context.Customers
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var customer = _bl.Get(id);
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,firstName,lastName,phoneNumber,email,address")] Customer customer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(customer);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(customer);
        //}

        // GET: Customers/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var customer = await _context.Customers.FindAsync(id);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(customer);
        //}

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        // GET: Customers/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var customer = await _context.Customers
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(customer);
        //}

        // POST: Customers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var customer = await _context.Customers.FindAsync(id);
        //    _context.Customers.Remove(customer);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool CustomerExists(int id)
        //{
        //    return _context.Customers.Any(e => e.Id == id);
        //}
    }
}
