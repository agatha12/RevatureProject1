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

namespace BankingApp.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            sessionSet();
            ViewData["Message"] = $"{sessionGetName()} {sessionGetId()}";
            return View();
        }

        public IActionResult Privacy()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public void sessionSet()
        {
             var name = User.Identity.Name;
            HttpContext.Session.SetString("Name", name);
            var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            HttpContext.Session.SetString("Id", id);
        }

        public string sessionGetName()
        {
            var val = HttpContext.Session.GetString("Name");
            return val;
        }

        public string sessionGetId()
        {
            var val = HttpContext.Session.GetString("Id");
            return val;
        }

    }

}
