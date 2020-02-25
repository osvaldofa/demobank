using DemoBank.Account.Domain.Interfaces;
using DemoBank.Account.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DemoBank.Account.Presentation.Controllers
{
    /// <summary>
    /// Controller responsible for manage Transactions API.
    /// </summary>
    [Route("api/v1/transaction")]
    [ApiController]
    public class TransactionController : ControllerBase
    {

        private readonly ILogger _logger;
        private ITransactionServices _transactionServices;

        /// <summary>
        /// Constructor method for Transaction Controller.
        /// </summary>
        /// <param name="transactionServices">Transaction services instantiated by dependency injection.</param>
        public TransactionController(ITransactionServices transactionServices, ILogger<TransactionController> logger)
        {
            this._transactionServices = transactionServices;
            this._logger = logger;
        }

        /// <summary>
        /// Create a new transaction.
        /// </summary>
        /// <param name="transaction">Transaction data.</param>
        /// <returns>Transaction Id.</returns>
        [HttpPost]
        public ActionResult<long> CreateTransaction(TransactionModel transaction)
        {
            long transactionId = 0;
            if (ValidateTransaction(transaction))
                transactionId = this._transactionServices.CreateTransaction(transaction);            

            if (transactionId == 0)
            {
                _logger.LogError("---> Transaction creation failed.");
                return BadRequest("Invalid transaction data.");
            }
            else
            {
                _logger.LogInformation("---> Transaction created for account " 
                    + transaction.DestinationAccount.AccountNumber 
                    + " - Type: " + transaction.TransactionType + " - Transaction Id: " + transactionId);
                return Created("transactionId", transactionId);
            }                
        }

        /// <summary>
        /// List all transactions for an specific account.
        /// </summary>
        /// <param name="accountNumber">Account number.</param>
        /// <returns>List of all transactions.</returns>
        [HttpGet("{accountNumber}")]
        public ActionResult<TransactionModel[]> GetTransactionsByAccountNumber(long accountNumber)
        {
            TransactionModel[] transactions = this._transactionServices.TransactionsByAccountNumber(accountNumber);
            if (transactions == null)
            {
                _logger.LogError("---> Transaction list failed to account " + accountNumber);
                return UnprocessableEntity();
            }                
            else
                return transactions;
        }

        #region "Support Methods"

        private bool ValidateTransaction(TransactionModel transaction)
        {
            if (ValidateTransactionDestinationAccount(transaction)
                && ValidateTransactionValue(transaction))
                return true;
            return false;
        }

        private bool ValidateTransactionDestinationAccount(TransactionModel transaction)
        {
            if (transaction?.DestinationAccount?.AccountNumber > 0)
                return true;
            return false;
        }

        private bool ValidateTransactionValue(TransactionModel transaction)
        {
            if (transaction?.Value > 0)
                return true;
            return false;
        }

        #endregion

    }
}
