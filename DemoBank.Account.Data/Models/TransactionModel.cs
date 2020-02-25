using System;
using System.Collections.Generic;
using System.Text;

namespace DemoBank.Account.Infrastructure.Data.Models
{
    public class TransactionModel
    {
        public long TransactionId { get; set; }
        public AccountModel DestinationAccount { get; set; }
        public AccountModel OriginAccount { get; set; }
        public double Value { get; set; }
        public string TransactionType { get; set; }
        public DateTime When { get; set; }

    }
}
