using DemoBank.Account.Domain.Interfaces;
using DemoBank.Account.Domain.Services;
using DemoBank.Account.Infrastructure.Data.Models;
using DemoBank.Account.Infrastructure.Data.Repositories;
using DemoBank.Account.Presentation.Controllers;
using DemoBank.CrossCutting.Enumerators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace DemoBank.Test
{
    [TestClass]
    public class DemoBankAPITest
    {
        private ICustomerRepository customerRepository;
        private ITransactionRepository transactionRepository;
        private IAccountRepository accountRepository;

        private IAccountService accountService;
        private ITransactionServices transactionServices;

        private AccountController accountController;
        private TransactionController transactionController;

        private ILogger<AccountController> accountLogger;
        private ILogger<TransactionController> transactionLogger;

        /// <summary>
        /// Initializes mock objects for test purposes.
        /// </summary>
        [TestInitialize]
        public void TestInitialization()
        {
            this.customerRepository = Substitute.For<ICustomerRepository>(); ;
            this.transactionRepository = Substitute.For<ITransactionRepository>(); ;
            this.accountRepository = Substitute.For<IAccountRepository>();

            this.accountService = new AccountService(this.accountRepository, this.customerRepository);
            this.transactionServices = new TransactionServices(this.transactionRepository, this.accountRepository);

            this.accountLogger = Substitute.For<ILogger<AccountController>>();
            this.transactionLogger = Substitute.For<ILogger<TransactionController>>();

            this.accountController = new AccountController(this.accountService, this.accountLogger);
            this.transactionController = new TransactionController(this.transactionServices, this.transactionLogger);
        }

        [TestCleanup]
        public void TestClean()
        {
            this.customerRepository = null;
            this.transactionRepository = null;
            this.accountRepository = null;

            this.accountService = null;
            this.transactionServices = null;
        }       


        #region TEST METHODS

        /// <summary>
        /// Test the creation of new account for a valid customer.
        /// </summary>
        [TestMethod]
        public void TestCreateNewAccountWithValidCustomer()
        {
            // Set account repository mock.
            CustomerModel customer = new CustomerModel(1010, "John", "Doe");
            AccountModel newAccount = new AccountModel(0, customer, 0);

            this.accountRepository.Save(newAccount).ReturnsForAnyArgs(1001);
            this.customerRepository.GetById(1010).Returns(customer);

            NewAccount envelope = new NewAccount();
            envelope.CustomerId = 1010;
            envelope.InitialCredit = 0;            

            Assert.IsInstanceOfType(this.accountController.CreateNewAccountForExistingUser(envelope).Result, typeof(CreatedResult));
        }

        /// <summary>
        /// Test the creation of new account for an invalid customer.
        /// </summary>
        [TestMethod]
        public void TestCreateNewAccountWithInvalidCustomer()
        {
            // Set account repository mock.
            CustomerModel customer = new CustomerModel(1010, "John", "Doe");
            AccountModel newAccount = new AccountModel(0, customer, 0);

            this.accountRepository.Save(newAccount).ReturnsForAnyArgs(1001);
            this.customerRepository.GetById(1010).ReturnsForAnyArgs(a => null);

            NewAccount envelope = new NewAccount();
            envelope.CustomerId = 1010;
            envelope.InitialCredit = 0;

            Assert.IsNotInstanceOfType(this.accountController.CreateNewAccountForExistingUser(envelope).Result, typeof(CreatedResult));
        }

        /// <summary>
        /// Test account search by account number.
        /// </summary>
        [TestMethod]
        public void TestGetAccountByAccountNumber()
        {
            // Set account repository mock.
            CustomerModel customer = new CustomerModel(1010, "John", "Doe");
            AccountModel account = new AccountModel(1101, customer, 100);

            this.accountRepository.GetById(1101).Returns(account);
            AccountModel actual = this.accountController.GetAccount(1101).Value;
            Assert.AreEqual(1101, actual.AccountNumber);
        }

        /// <summary>
        /// Test account search by account number.
        /// </summary>
        [TestMethod]
        public void TestGetAccountByInvalidAccountNumber()
        {
            // Set account repository mock.
            CustomerModel customer = new CustomerModel(1010, "John", "Doe");
            AccountModel account = new AccountModel(1101, customer, 100);
            this.accountRepository.GetById(1101).Returns(account);

            Assert.AreEqual(null, this.accountController.GetAccount(1100).Value);
        }

        /// <summary>
        /// Test the creation of a new transaction for an existent account.
        /// </summary>
        [TestMethod]
        public void TestCreateTransactionValidDeposit()
        {
            // Set account repository mock.
            CustomerModel customer = new CustomerModel(1010, "John", "Doe");
            AccountModel account = new AccountModel(1101, customer, 100);            

            TransactionModel transaction = new TransactionModel();
            transaction.DestinationAccount = account;
            transaction.TransactionType = TransactionTypes.DEPOSIT;
            transaction.Value = 100;

            this.accountRepository.GetById(1101).Returns(account);
            this.transactionRepository.Save(transaction).ReturnsForAnyArgs(11001);

            Assert.IsInstanceOfType(this.transactionController.CreateTransaction(transaction).Value, typeof(CreatedResult));
        }

        /// <summary>
        /// Test the creation of a new transaction for a non existent account.
        /// </summary>
        [TestMethod]
        public void TestCreateTransactionValidDepositInInvalidAccount()
        {
            // Set account repository mock.
            CustomerModel customer = new CustomerModel(1010, "John", "Doe");
            AccountModel account = new AccountModel(1101, customer, 100);

            TransactionModel transaction = new TransactionModel();
            transaction.DestinationAccount = account;
            transaction.TransactionType = TransactionTypes.DEPOSIT;
            transaction.Value = 100;
                        
            this.transactionRepository.Save(transaction).ReturnsForAnyArgs(11001);

            Assert.IsNotInstanceOfType(this.transactionController.CreateTransaction(transaction).Value, typeof(CreatedResult));
        }

        [TestMethod]
        public void TestGetTransactionsByAccountNumber()
        { 

        }

        #endregion

    }
}
