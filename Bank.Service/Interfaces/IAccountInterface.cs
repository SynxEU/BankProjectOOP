using Bank.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Service.Interfaces
{
    public interface IAccountInterface
    {
        bool CreateAccount(AccountModel account, int userId);
        bool DeleteAccount(AccountModel account);
        List<AccountModel> GetAllAccounts();
        AccountModel GetAccountByNumber(AccountModel account);
        List<AccountModel> GetAccountsAtUsers(int userId);
        bool WithdrawAccount(AccountModel account, decimal withdraw);
        bool DepositAccount(AccountModel account, decimal deposit);
    }
}
