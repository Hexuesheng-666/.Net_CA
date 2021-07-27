using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class SQLCustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext appDbContext;

        public SQLCustomerRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public Customer Add(Customer customer)
        {
            appDbContext.Customers.Add(customer);
            appDbContext.SaveChanges();
            return customer;
        }
    }
}
