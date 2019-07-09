using BankEntities;
using BankingApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BankingApp.BusinessLayer
{
    public interface ICustomerBL
    {
        IList<Customer> Get();
        Customer Get(int? id);
        void Add(Customer customer);
       
    }
   

    public class CustomerBL : ICustomerBL
    {
        private BankClientContext _context;



        public CustomerBL()
        {
            var options = new DbContextOptionsBuilder<BankClientContext>().Options;
            _context = new BankClientContext(options);
        }

        public CustomerBL(BankClientContext context)
        {
            _context = context;
        }

        public IList<Customer> Get()
        {
            var customers = _context.Customers.ToList();

            return customers;
        }

        public Customer Get(int? id)
        {
            var customer = _context.Customers.Find(id);

            return customer;
        }

        public void Add(Customer customer)
        {



            

            _context.Customers.Add(customer);
           _context.SaveChangesAsync();
            

            //return customer;
        }

    }
}
