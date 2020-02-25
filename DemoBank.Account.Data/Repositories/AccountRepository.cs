using System.Collections.Generic;
using System.Linq;
using DemoBank.Account.Infrastructure.Data.Models;

namespace DemoBank.Account.Infrastructure.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private List<AccountModel> Accounts;

        /// <summary>
        /// Constructor method responsible for repository initialization.
        /// </summary>
        public AccountRepository()
        {
            this.Accounts = new List<AccountModel>();

            // Demo data
            CustomerModel customerA = new CustomerModel(1010, "Bono", "Vox");
            CustomerModel customerB = new CustomerModel(1011, "Robert", "Stips");
            CustomerModel customerC = new CustomerModel(1012, "Marco", "Vrolijk");

            this.Accounts.Add(new AccountModel(1101, customerA, 20.45));
            this.Accounts.Add(new AccountModel(1102, customerB, 2520.10));
            this.Accounts.Add(new AccountModel(1103, customerC, 10435.92));
        }

        /// <summary>
        /// Get an Account object based on Account Number.
        /// </summary>
        /// <param name="accountNumber">The retrieved account, or null if not found.</param>
        /// <returns></returns>
        public AccountModel GetById(long accountNumber)
        {
            if (accountNumber == 0)
                return null;
            else 
            {
                // Filter elements in list using LINQ extension methods.
                var accounts = from a in this.Accounts
                               where a.AccountNumber == accountNumber
                               select a;
                if (accounts.ToList<AccountModel>().Count == 0)
                {
                    return null;
                }
                else
                    return accounts?.First();
            }            
        }

        public long Save(AccountModel account)
        {

            if (GetById(account.AccountNumber) != null) // Account exists and will be updated.
            {
                AccountModel accountExistent = this.Accounts.Find(a => a.AccountNumber == account.AccountNumber);
                CopyAccount(account, accountExistent);
                return account.AccountNumber;
            }
            else // Account no longer exists and will be created.
            {
                // Get the MAX account number.
                long max = this.Accounts.Select(a => a.AccountNumber).Max();

                account.AccountNumber = max + 1;
                this.Accounts.Add(account);

                return account.AccountNumber;
            }            
        }

        private void CopyAccount(AccountModel accountA, AccountModel accountB)
        {
            accountB.Balance = accountA.Balance;
        }
    }
}
