using DemoBank.Account.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoBank.Account.Infrastructure.Data.Repositories
{
    public interface ITransactionRepository
    {
        long Save(TransactionModel transaction);
        TransactionModel[] GetTransactionsByAccountNumber(long accountNumber);
    }
}
