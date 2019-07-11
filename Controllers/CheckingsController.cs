﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BankEntities;
using BankingApp.Models;
using Microsoft.AspNetCore.Http;
using BankingApp.DAL;

namespace BankingApp.Controllers
{
    public class CheckingsController : Controller
    {
        private readonly BankClientContext _context;

        public CheckingsController(BankClientContext context)
        {
            _context = context;
        }

        // GET: Checkings
        public async Task<IActionResult> Index()
        {
            return View(await _context.Checking.ToListAsync());
        }

        public async Task<IActionResult> MyChecking()
        {

            
            var id = sessionGetId();

            var check = await _context.Checking.Where(c => c.CustomerId == id).ToListAsync();

            
            if (check.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else {
                return View(check);
            }

            
        }

        public IActionResult Withdraw(int id)
        {

            //var check = await _context.Checking.FirstOrDefaultAsync(c => c.Id == id);

            ViewData["Id"] = id;
                return View();
            
    
           
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdraw(int id, int amount)
        {
    

            try
            {
                Checking checking = new Checking();
                checking = await _context.Checking.FirstOrDefaultAsync(c => c.Id == id);

                if (checking.Balance < amount)
                {
                    ViewData["ErrorMessage"] = $"You tried to withdraw ${amount} but your balance is only ${checking.Balance}";
                    return View();
                }
                else {
                    var newBalance = (checking.Balance - amount);
                    checking.Balance = newBalance;


                    _context.Update(checking);
                    await _context.SaveChangesAsync();
                }
            }
            catch 
            {
                ViewData["ErrorMessage"] = "There was a problem with your withdrawl please try again";
                return View();
            }
            return RedirectToAction(nameof(MyChecking));

            
        }

        public IActionResult Deposit(int id)
        {

            //var check = await _context.Checking.FirstOrDefaultAsync(c => c.Id == id);

            ViewData["Id"] = id;
            return View();



        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit(int id, int amount)
        {


            try
            {
                Checking checking = new Checking();
                checking = await _context.Checking.FirstOrDefaultAsync(c => c.Id == id);


                    var newBalance = (checking.Balance + amount);
                    checking.Balance = newBalance;


                    _context.Update(checking);
                    await _context.SaveChangesAsync();
                
            }
            catch
            {
                ViewData["ErrorMessage"] = "There was a problem with your withdrawl please try again";
                return View();
            }
            return RedirectToAction(nameof(MyChecking));


        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Withdraw(int id, int amount, [Bind("Id,type,CustomerId,InterestRate,Balance")] Checking checking)
        //{


        //    if (id != checking.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var Newamount = amount;
        //            _context.Update(checking);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CheckingExists(checking.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(checking);
        //}

        // GET: Checkings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checking = await _context.Checking
                .FirstOrDefaultAsync(m => m.Id == id);
            if (checking == null)
            {
                return NotFound();
            }

            return View(checking);
        }

        // GET: Checkings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Checkings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,type,CustomerId,InterestRate,Balance")] Checking checking)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    checking.type = "checking";
                    checking.CustomerId = (int)sessionGetId();
                    checking.InterestRate = 2.2;
                    _context.Add(checking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("MyChecking", "Checkings");
                }
            }
            catch
            {
               
            }
            return View(checking);
        }

        // GET: Checkings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checking = await _context.Checking.FindAsync(id);
            if (checking == null)
            {
                return NotFound();
            }
            return View(checking);
        }

        // POST: Checkings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,type,CustomerId,InterestRate,Balance")] Checking checking)
        {
            if (id != checking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(checking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CheckingExists(checking.Id))
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
            return View(checking);
        }

        // GET: Checkings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checking = await _context.Checking
                .FirstOrDefaultAsync(m => m.Id == id);
            if (checking == null)
            {
                return NotFound();
            }

            return View(checking);
        }

        // POST: Checkings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var checking = await _context.Checking.FindAsync(id);
            _context.Checking.Remove(checking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CheckingExists(int id)
        {
            return _context.Checking.Any(e => e.Id == id);
        }

        public int? sessionGetId()
        {
            var val = HttpContext.Session.GetInt32("Id");
            return val;
        }
    }
}
