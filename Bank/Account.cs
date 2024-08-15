using Bank.Domain.Models;
using Bank.Service.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class Account
    {
        AccountMethod _accounts;
        public Account()
        { 
            _accounts = new AccountMethod();
        }
        public bool CreateAccount(AccountModel account, int userId)
        { 
            return _accounts.CreateAccount(account, userId);
        }
    }
}
