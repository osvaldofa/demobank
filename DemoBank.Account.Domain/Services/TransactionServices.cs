using System;
using System.Collections.Generic;
using System.Text;
using DemoBank.Account.Domain.Interfaces;
using DemoBank.Account.Infrastructure.Data.Repositories;
using DemoBank.Account.Infrastructure.Data.Models;
using DemoBank.CrossCutting.Enumerators;

namespace DemoBank.Account.Domain.Services
{
    public class TransactionServices : ITransactionServices
    {
        private ITransactionRepository _transactionRepository;
        private IAccountRepository _accountRepository;

        /// <summary>
        /// Constructor method.
        /// </summary>
        /// <param name="transactionRepository">Transaction repository instantiated by dependency injection.</param>
        /// <param name="accountRepository">Account repository instantiated by dependency injection.</param>
        public TransactionServices(ITransactionRepository transactionRepository, IAccountRepository accountRepository)
        {
            this._transactionRepository = transactionRepository;
            this._accountRepository = accountRepository;
        }

        public long CreateTransaction(TransactionModel transaction)
        {
            switch (transaction.TransactionType)
            {
                case TransactionTypes.DEPOSIT:
                    return CreateDepositTransaction(transaction);
                case TransactionTypes.WITHDRAW:
                    return CreateWithdrawTransaction(transaction);
                case TransactionTypes.TRANSFER:
                    return CreateTransferTransaction(transaction);
                default:
                    return 0;
            }
            throw new NotImplementedException();
        }

        public TransactionModel[] TransactionsByAccountNumber(long accountNumber)
        {
            if (AccountExists(accountNumber))
                return this._transactionRepository.GetTransactionsByAccountNumber(accountNumber);
            return null;
        }

        private long CreateDepositTransaction(TransactionModel transaction)
        {
            if (AccountExists(transaction?.DestinationAccount) && transaction.Value > 0)
            {
                // Transaction registration.
                transaction.When = DateTime.Now;
                long transactionId = this._transactionRepository.Save(transaction);

                // Account update.
                AccountModel account = this._accountRepository.GetById(transaction.DestinationAccount.AccountNumber);
                account.Balance += transaction.Value;
                this._accountRepository.Save(account);
                
                return transactionId;
            }                
            return 0;
        }

        private long CreateWithdrawTransaction(TransactionModel transaction)
        {
            if (AccountExists(transaction?.DestinationAccount)
                && AccountEnoughtBalance(transaction.DestinationAccount, transaction.Value))
            {
                // Transaction registration.
                transaction.When = DateTime.Now;
                long transactionId = this._transactionRepository.Save(transaction);

                // Account update.
                AccountModel account = this._accountRepository.GetById(transaction.DestinationAccount.AccountNumber);
                account.Balance -= transaction.Value;
                this._accountRepository.Save(account);

                return transactionId;
            }
            return 0;
        }

        private long CreateTransferTransaction(TransactionModel transaction)
        {
            if (AccountExists(transaction.DestinationAccount)
                && AccountExists(transaction.OriginAccount)
                && AccountEnoughtBalance(transaction.OriginAccount, transaction.Value))
            {
                // Transaction registration.
                transaction.When = DateTime.Now;
                long transactionId = this._transactionRepository.Save(transaction);

                // Account update.
                AccountModel originAccount = this._accountRepository.GetById(transaction.OriginAccount.AccountNumber);
                originAccount.Balance -= transaction.Value;
                this._accountRepository.Save(originAccount);

                AccountModel destinationAccount = this._accountRepository.GetById(transaction.DestinationAccount.AccountNumber);
                destinationAccount.Balance += transaction.Value;
                this._accountRepository.Save(destinationAccount);

                return transactionId;
            }
            return 0;
        }

        private bool AccountExists(AccountModel account)
        {
            if (account?.AccountNumber > 0
                && this._accountRepository.GetById(account.AccountNumber) != null)
                return true;
            return false;
        }

        private bool AccountExists(long accountNumber)
        {
            if (accountNumber > 0
                && this._accountRepository.GetById(accountNumber) != null)
                return true;
            return false;
        }

        private bool AccountEnoughtBalance(AccountModel account, double value)
        {
            AccountModel accountFound = this._accountRepository.GetById(account.AccountNumber);
            if (accountFound?.Balance > value)
                return true;
            return false;
        }
    }
}
