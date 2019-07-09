using BankEntities;
using BankingApp.Models;
//using Fluent.Infrastructure.FluentModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BankingApp.BusinessLayer
{
    public interface ICustomerBL
    {
        IList<Customer> Get();
        Customer Get(int? id);
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

        public Customer Post()
        {
            var customer = new Customer();

            //customer.email = User.Identity.Name;

            return customer;
        }
    }
}
