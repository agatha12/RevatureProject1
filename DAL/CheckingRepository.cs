using BankEntities;
using BankingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.DAL
{
    public class CheckingRepository : ICheckingRepository
    {
        private BankClientContext context;

        

        public CheckingRepository(BankClientContext context)
        {
            this.context = context;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
        public List<Checking> GetCheckingbyId(int id)
        {
            List<Checking> checking = null;
            try
            {

                checking = context.Checking.Where(check => check.CustomerId == id).ToList();
            }
            catch
            {

            }

            return checking;
        }



        //public Customer GetCustomerByEmail(string email)
        //{
        //    Customer customer = null;
        //    try
        //    {
        //        customer = context.Customers.Where(c => c.email == email).First<Customer>();
        //    }
        //    catch
        //    {

        //    }

        //    return customer;
        //}
        //public Customer GetCustomerById(int? id)
        //{
        //    Customer customer = null;
        //    try
        //    {
        //        customer = context.Customers.Find(id);
        //    }
        //    catch
        //    {

        //    }

        //    return customer;
        //}
    }
}
