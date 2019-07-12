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
            try
            {


                var id = sessionGetId();

                var check = await _context.Checking.Where(c => c.CustomerId == id).ToListAsync();


                if (check.Count == 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(check);
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
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

                  
                    Transaction transaction = new Transaction();
                    transaction.accountId = id;
                    transaction.accountType = "checking";
                    transaction.amount = amount;
                    transaction.date = DateTime.Now;
                    transaction.type = "withdraw";

                    _context.Update(checking);
                    await _context.SaveChangesAsync();


                    _context.Update(transaction);
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

                Transaction transaction = new Transaction();
                transaction.accountId = id;
                transaction.accountType = "checking";
                transaction.amount = amount;
                transaction.date = DateTime.Now;
                transaction.type = "deposit";

                _context.Update(checking);
                await _context.SaveChangesAsync();


                _context.Update(transaction);
                await _context.SaveChangesAsync();


             
                
            }
            catch
            {
                ViewData["ErrorMessage"] = "There was a problem with your deposit please try again";
                return View();
            }
            return RedirectToAction(nameof(MyChecking));


        }

        public IActionResult Transfer(int id)
        {


            ViewData["Id"] = id;
            return View();



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transfer(int id, int amount, int tid, string type)
        {


            try
            {
                Checking checking = new Checking();
                checking = await _context.Checking.FirstOrDefaultAsync(c => c.Id == id);

                if (type == "checking")
                {
                    Checking tochecking = new Checking();
                    tochecking = await _context.Checking.FirstOrDefaultAsync(c => c.Id == tid);

                    if (tochecking != null)
                    {
                        if (checking.CustomerId != tochecking.CustomerId)
                        {
                            ViewData["ErrorMessage"] = $"You can only transfer between your own accounts";
                            return View();
                        }
                        else if (checking.Balance < amount)
                        {
                            ViewData["ErrorMessage"] = $"You tried to transfer ${amount} but your balance is only ${checking.Balance}";
                            return View();
                        }
                        else
                        {
                            var newBalance = (checking.Balance - amount);
                            checking.Balance = newBalance;

                            Transaction transaction = new Transaction();
                            transaction.accountId = id;
                            transaction.accountType = "checking";
                            transaction.amount = amount;
                            transaction.date = DateTime.Now;
                            transaction.type = "transer out";

                            _context.Update(checking);
                            await _context.SaveChangesAsync();


                            _context.Update(transaction);
                            await _context.SaveChangesAsync();


                            var tonewBalance = (tochecking.Balance + amount);
                            tochecking.Balance = tonewBalance;

                            Transaction totransaction = new Transaction();
                            totransaction.accountId = tid;
                            totransaction.accountType = "checking";
                            totransaction.amount = amount;
                            totransaction.date = DateTime.Now;
                            totransaction.type = "transfer in";

                            _context.Update(tochecking);
                            await _context.SaveChangesAsync();


                            _context.Update(totransaction);
                            await _context.SaveChangesAsync();


                        }
                    }
                    else
                    {
                        ViewData["ErrorMessage"] = $"Please enter a valid account to transfer into.";
                        return View();
                    }
                }
                else
                {
                    Business business = new Business();
                    business = await _context.Business.FirstOrDefaultAsync(c => c.Id == tid);



                    if (business != null)
                    {
                        if (checking.CustomerId != business.CustomerId)
                        {
                            ViewData["ErrorMessage"] = $"You can only transfer between your own accounts";
                            return View();
                        }
                        else if (checking.Balance < amount)
                        {
                            ViewData["ErrorMessage"] = $"You tried to transfer ${amount} but your balance is only ${checking.Balance}";
                            return View();
                        }
                        else
                        {
                            var newBalance = (checking.Balance - amount);
                            checking.Balance = newBalance;

                            Transaction transaction = new Transaction();
                            transaction.accountId = id;
                            transaction.accountType = "checking";
                            transaction.amount = amount;
                            transaction.date = DateTime.Now;
                            transaction.type = "transer out";


                            _context.Update(checking);
                            await _context.SaveChangesAsync();


                            _context.Update(transaction);
                            await _context.SaveChangesAsync();


                            var tonewBalance = (business.Balance + amount);
                            business.Balance = tonewBalance;

                            Transaction totransaction = new Transaction();
                            totransaction.accountId = tid;
                            totransaction.accountType = "business";
                            totransaction.amount = amount;
                            totransaction.date = DateTime.Now;
                            totransaction.type = "transfer in";


                            _context.Update(business);
                            await _context.SaveChangesAsync();

                            _context.Update(totransaction);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        ViewData["ErrorMessage"] = $"Please enter a valid account to transfer into.";
                        return View();
                    }
                }
            }
            catch
            {
                ViewData["ErrorMessage"] = "There was a problem with your withdrawl please try again";
                return View();
            }
            return RedirectToAction(nameof(MyChecking));


        }


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


            if (checking.Balance != 0)
            {
                ViewData["ErrorMessage"] = "You can not delete an account unless you balance is $0";
                return View();
            }
            else
            {
                _context.Checking.Remove(checking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyChecking));
            }
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
