using System;
using System.Collections.Generic;
using System.Text;

namespace DemoBank.Account.Infrastructure.Data.Models
{
    public class CustomerModel
    {
        public long CustomerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }


        /// <summary>
        /// Constructor method assigning attributes values during instantiation.
        /// </summary>
        /// <param name="customerId">Customer Id</param>
        /// <param name="name">Customer name.</param>
        /// <param name="surname">Customer surname.</param>
        public CustomerModel(long customerId, string name, string surname)
        {
            this.CustomerId = customerId;
            this.Name = name;
            this.Surname = surname;
        }
    }
}
