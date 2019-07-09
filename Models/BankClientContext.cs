using BankEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.Models
{
    public class BankClientContext : DbContext
    {
        public BankClientContext(DbContextOptions<BankClientContext> context) : base(context)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Checking> Checking { get; set; }
        public DbSet<Business> Business { get; set; }
        public DbSet<Term> Term { get; set; }
        public DbSet<Loan> Loan { get; set; }

        public DbSet<Transaction> Transaction { get; set; }

    }
}
