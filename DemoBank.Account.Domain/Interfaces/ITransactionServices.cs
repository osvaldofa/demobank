using DemoBank.Account.Infrastructure.Data.Models;

namespace DemoBank.Account.Domain.Interfaces
{
    public interface ITransactionServices
    {
        long CreateTransaction(TransactionModel transaction);
        TransactionModel[] TransactionsByAccountNumber(long AccountNumber);
    }
}
