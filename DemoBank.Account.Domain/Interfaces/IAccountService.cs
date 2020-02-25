using System;
using System.Collections.Generic;
using System.Text;
using DemoBank.Account.Infrastructure.Data.Models;

namespace DemoBank.Account.Domain.Interfaces
{
    public interface IAccountService
    {
        long CreateNewAccountForExistingUser(NewAccount account);
        AccountModel GetAccountById(long accountId);
    }
}
