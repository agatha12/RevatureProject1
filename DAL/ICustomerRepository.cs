using BankEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.DAL
{
    public interface ICustomerRepository : IDisposable
    {
        Customer GetCustomerByEmail(string email);
    }
}
