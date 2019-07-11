using BankEntities;
using BankingApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.DAL
{
    public class CustomerRepository : ICustomerRepository
    {
        private BankClientContext context;

        //private BankClientContext context = new BankClientContext();

        public CustomerRepository(BankClientContext context)
        {
            this.context = context;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public Customer GetCustomerByEmail(string email)
        {
            Customer customer = null;
            try
            {
                 customer = context.Customers.Where(c => c.email == email).First<Customer>();
            }
            catch
            {
                 
            }

            return customer;
        }
        public Customer GetCustomerById(int? id)
        {
            Customer customer = null;
            try
            {
                customer = context.Customers.Find(id);
            }
            catch
            {

            }

            return customer;
        }
    }
}
