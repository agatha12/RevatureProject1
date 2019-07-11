using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankingApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using BankingApp.DAL;

namespace BankingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly BankClientContext _context;
        public HomeController(BankClientContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var name = User.Identity.Name;
            if (name == null)
            {
                return View();
            }
            else
            {

                var custrepo = new CustomerRepository(_context);

                var cust = custrepo.GetCustomerByEmail(name);

                if (cust == null)
                {
                    return RedirectToAction("Create", "Customers");
                }
                else
                {
                    sessionSet(cust.Id);


                    ViewData["Fname"] = cust.firstName;
                    ViewData["Lname"] = cust.lastName;



                   

                    return View("Menu");
                }
            }
        }

        public IActionResult Privacy()
        {

            return View();
        }

        public IActionResult Menu()
        {
            var custrepo = new CustomerRepository(_context);

            var cust = custrepo.GetCustomerById(sessionGetId());

            if (cust == null)
            {
                return View("Index");
            }
            else
            {
                ViewData["Fname"] = cust.firstName;
                ViewData["Lname"] = cust.lastName;



                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public void sessionSet(int id)
        {
             var name = User.Identity.Name;
            HttpContext.Session.SetString("Name", name);
            //var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            HttpContext.Session.SetInt32("Id", id);

        }

        public string sessionGetName()
        {
            var val = HttpContext.Session.GetString("Name");
            return val;
        }

        public int? sessionGetId()
        {
            var val = HttpContext.Session.GetInt32("Id");
            return val;
        }

    }

}
