using System;
using System.Collections.Generic;
using System.Linq;
using DemoBank.Account.Infrastructure.Data.Models;

namespace DemoBank.Account.Infrastructure.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private List<CustomerModel> Customers;

        public CustomerRepository() 
        {
            this.Customers = new List<CustomerModel>();
            this.Customers.Add(new CustomerModel(1010, "Bono", "Vox"));
            this.Customers.Add(new CustomerModel(1011, "Robert", "Stips"));
            this.Customers.Add(new CustomerModel(1012, "Marco", "Vrolijk "));
        }

        /// <summary>
        /// Get a customer filtering by customer id.
        /// </summary>
        /// <param name="customerId">Customer id.</param>
        /// <returns>The Customer found or a null object if not found.</returns>
        public CustomerModel GetById(long customerId)
        {
            var customers = from c in this.Customers
                            where c.CustomerId == customerId
                            select c;

            return customers.First();
        }
    }
}
