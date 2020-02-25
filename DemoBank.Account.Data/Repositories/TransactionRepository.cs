using DemoBank.Account.Infrastructure.Data.Models;
using DemoBank.CrossCutting.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoBank.Account.Infrastructure.Data.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private List<TransactionModel> Transactions;

        public TransactionRepository()
        {
            this.Transactions = new List<TransactionModel>();

            TransactionModel transaction = new TransactionModel();
            CustomerModel customer = new CustomerModel(1010, "Bono", "Vox");
            AccountModel destinationAccount = new AccountModel(1101, customer, 0);

            transaction.DestinationAccount = destinationAccount;
            transaction.TransactionId = 110001;
            transaction.Value = 250;
            transaction.TransactionType = TransactionTypes.DEPOSIT;
            transaction.When = DateTime.Now;

            this.Transactions.Add(transaction);
        }

        /// <summary>
        /// Retrieve a list of transactions filtered by destination account number.
        /// </summary>
        /// <param name="accountNumber">The account number to filter transactions.</param>
        /// <returns>The transaction list.</returns>
        public TransactionModel[] GetTransactionsByAccountNumber(long accountNumber)
        {
            var transactions = from t in this.Transactions
                               where t.DestinationAccount.AccountNumber == accountNumber
                               select t;
            return transactions.ToArray();
        }

        /// <summary>
        /// Create a new transaction.
        /// </summary>
        /// <param name="transaction">The new transaction data.</param>
        /// <returns>The id of created transaction, confirming the register.</returns>
        public long Save(TransactionModel transaction)
        {
            long newId = this.Transactions.Select(t => t.TransactionId).Max() + 1;
            transaction.TransactionId = newId;
            this.Transactions.Add(transaction);

            return newId;
        }
    }
}
