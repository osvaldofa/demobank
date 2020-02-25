using DemoBank.Account.Domain.Interfaces;
using DemoBank.Account.Infrastructure.Data.Models;
using DemoBank.Account.Infrastructure.Data.Repositories;

namespace DemoBank.Account.Domain.Services
{
    public class AccountService : IAccountService
    {
        // Data repository for Account objects.
        private IAccountRepository _accountRepository;

        // Data repository for Customer objects.
        private ICustomerRepository _customerRepository;

        /// <summary>
        /// Constructor method using dependency injection to instantiate local reference to Account Repository.
        /// </summary>
        /// <param name="accountRepository">Account Repository instantiated by dependency injection.</param>
        public AccountService(IAccountRepository accountRepository, ICustomerRepository customerRepository)
        {
            this._accountRepository = accountRepository;
            this._customerRepository = customerRepository;
        }

        /// <summary>
        /// Create a new account for an existing customer.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public long CreateNewAccountForExistingUser(NewAccount account)
        {
            CustomerModel customer = _customerRepository.GetById(account.CustomerId);
            if (customer != null)
            {
                if (account.InitialCredit > 0)
                {
                    AccountModel newAccount = new AccountModel(0, customer, 0);
                    long accountNumber = this._accountRepository.Save(newAccount);

                    // Create a deposit transaction.

                    return accountNumber;
                }
                else
                {
                    AccountModel newAccount = new AccountModel(0, customer, 0);
                    long accountNumber = this._accountRepository.Save(newAccount);
                    return accountNumber;
                }                
            }
            return 0;
        }

        /// <summary>
        /// Search an existing account by account number.
        /// </summary>
        /// <param name="accountId">Account number.</param>
        /// <returns></returns>
        public AccountModel GetAccountById(long accountId)
        {
            return this._accountRepository.GetById(accountId);
        }
    }
}
