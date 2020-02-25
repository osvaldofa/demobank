using System;
using System.Collections.Generic;
using System.Text;
using DemoBank.Account.Infrastructure.Data.Models;

namespace DemoBank.Account.Infrastructure.Data.Repositories
{
    public interface IAccountRepository
    {
        long Save(AccountModel account);
        AccountModel GetById(long AccountNumber);        
    }
}
