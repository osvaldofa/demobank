namespace DemoBank.Account.Infrastructure.Data.Models
{
    public class AccountModel
    {
        public long AccountNumber { get; set; }
        public CustomerModel CustomerReference { get; set; }
        public double Balance { get; set; }

        /// <summary>
        /// Constructor method assigning the attributes values during instantiation.
        /// </summary>
        /// <param name="accountNumber">Account Number.</param>
        /// <param name="customer">Customer assigned to this account.</param>
        /// <param name="balance">Initial balance for this account.</param>
        public AccountModel(long accountNumber, CustomerModel customer, double balance)
        {
            this.AccountNumber = accountNumber;
            this.CustomerReference = customer;
            this.Balance = balance;
        }
    }
}
