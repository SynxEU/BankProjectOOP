using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Models
{
    public class AccountType
    {
        public enum Types
        {
            Checking_Account = 0,
            Consumer_Account = 1,
            Savings_Account = 2
        }
    }
}
