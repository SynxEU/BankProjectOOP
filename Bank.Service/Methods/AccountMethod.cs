using Bank.Domain.Connections;
using Bank.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Service.Methods
{
    public class AccountMethod
    {
        Accounts _accountConnection;

        public AccountMethod()
        {
            _accountConnection = new Accounts();
        }

        public bool CreateAccount(AccountModel account)
            => _accountConnection.CreateAccount(account);
        public bool DeleteAccount(AccountModel account)
            => _accountConnection.DeleteAccount(account);
        public List<AccountModel> GetAllAccounts()
            => _accountConnection.GetAllAccounts();
        public AccountModel GetAccountByNumber(AccountModel account)
            => _accountConnection.GetAccountByNumber(account);
        public List<AccountModel> GetAccountsAtUsers(int userId)
            => _accountConnection.GetAccountsAtUsers(userId);
        public bool WithdrawAccount(AccountModel account, decimal withdraw)
            => _accountConnection.WithdrawAccount(account, withdraw);
        public bool DepositAccount(AccountModel account, decimal deposit)
            => _accountConnection.DepositAccount(account, deposit);
    }
}
