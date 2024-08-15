using Bank.Domain.Models;
using Microsoft.Data.SqlClient;

namespace Bank.Domain.Connections
{
    public class Accounts
    {
        private readonly Sql _sql;
        public Accounts()
        {
            _sql = new Sql();
        }

        #region Create Account
        /// <summary>
        /// Creates Account
        /// </summary>
        /// <param name="account"></param>
        /// <returns>True if successful, false if not</returns>
        public bool CreateAccount(AccountModel account)
        {
            SqlCommand cmd = _sql.Execute("sp_CreateAccount");
            cmd.Parameters.AddWithValue("TitleID", account.TitleID);
            switch (account.TitleID)
            {
                case 0:
                    account.InterestRate = 1.005;
                    break;
                case 1:
                    account.InterestRate = 1.01;
                    break;
                case 2:
                    account.InterestRate = 1.001;
                    break;
                default:
                    break;
            }
            cmd.Parameters.AddWithValue("Interest", account.InterestRate);

            try
            {
                cmd.Connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (SqlException ex)
            {
                Console.Out.WriteLine(ex.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }

            return false;
        }

        #endregion

        #region Delete Account
        /// <summary>
        /// Deletes account
        /// </summary>
        /// <param name="account"></param>
        /// <returns>True if successful, false if not</returns>
        public bool DeleteAccount(AccountModel account)
        {
            SqlCommand cmd = _sql.Execute("sp_DeleteAccount");
            cmd.Parameters.AddWithValue("accountNumber", account.AccountNumber);

            try
            {
                cmd.Connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (SqlException ex)
            {
                Console.Out.WriteLine(ex.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }

            return false;
        }
        #endregion

        #region Get all Accounts
        /// <summary>
        /// Get All Accounts
        /// </summary>
        /// <returns>All accounts from database</returns>
        public List<AccountModel> GetAllAccounts()
        {
            SqlCommand cmd = _sql.Execute("sp_GetAllAccounts");

            try
            {
                cmd.Connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                List<AccountModel> account = new List<AccountModel>();

                while (reader.Read())
                {
                    account.Add(new AccountModel()
                    {
                        AccountNumber = reader.GetInt32(0),
                        TitleID = reader.GetInt32(1),
                        Balance = reader.GetInt32(2),
                        InterestRate = reader.GetDecimal(3)
                    });
                }
                return account;
            }
            catch (SqlException ex)
            {
                Console.Out.WriteLine(ex.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }

            return null;
        }
        #endregion

        #region Get Account By Number
        /// <summary>
        /// Gets account from account number
        /// </summary>
        /// <param name="account"></param>
        /// <returns>account details</returns>
        public AccountModel GetAccountByNumber(AccountModel account)
        {
            SqlCommand cmd = _sql.Execute("sp_GetAccountByNumber");
            cmd.Parameters.AddWithValue("AccountNumber", account.AccountNumber);

            try
            {
                cmd.Connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    account.AccountNumber = reader.GetInt32(0);
                    account.TitleID = reader.GetInt32(1);
                    account.Balance = reader.GetInt32(2);
                    account.InterestRate = reader.GetDecimal(3);
                }
                return account;
            }
            catch (SqlException ex)
            {
                Console.Out.WriteLine(ex.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }

            return null;
        }
        #endregion

        #region Get All Accounts By User ID
        /// <summary>
        /// Get all accounts thats connected to one user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Users accounts</returns>
        public List<AccountModel> GetAccountsAtUsers(int userId)
        {
            SqlCommand cmd = _sql.Execute("sp_GetAccountsByUserID");
            cmd.Parameters.AddWithValue("UserId", userId);

            try
            {
                cmd.Connection.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                List<AccountModel> accountsUsers = new List<AccountModel>();

                while (dr.Read())
                {
                    accountsUsers.Add(new AccountModel()
                    {
                        AccountNumber = dr.GetInt32(1)
                    });
                }

                foreach (AccountModel accounts in accountsUsers)
                {
                    SqlCommand cmd2 = _sql.Execute("sp_GetAccountByNumber");
                    cmd2.Parameters.AddWithValue("AccountNumber", accounts.AccountNumber);

                    SqlDataReader reader = cmd2.ExecuteReader();
                    List<AccountModel> account = new List<AccountModel>();

                    while (reader.Read())
                    {
                        account.Add(new AccountModel()
                        {
                            AccountNumber = reader.GetInt32(0),
                            TitleID = reader.GetInt32(1),
                            Balance = reader.GetInt32(2),
                            InterestRate = reader.GetDecimal(3)
                        });
                    }
                    return account;
                }
            }
            catch (SqlException ex)
            {
                Console.Out.WriteLine(ex.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }

            return null;
        }
        #endregion

        #region Withdraw
        /// <summary>
        /// Withdraws from account if they withdraw less than their balance
        /// </summary>
        /// <param name="account"></param>
        /// <param name="withdraw"></param>
        /// <returns>True if successful and false if not</returns>
        public bool WithdrawAccount(AccountModel account, decimal withdraw)
        {
            SqlCommand cmd = _sql.Execute("sp_WithDraw");

            string? result;
            bool verify = false;

            cmd.Parameters.AddWithValue("AccountNumber", account.AccountNumber);
            cmd.Parameters.AddWithValue("Withdraw", withdraw);

            try
            {
                cmd.ExecuteNonQuery();
                SqlDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    result = reader["Result"].ToString();
                    switch (result)
                    {
                        case "You have now withdrawen":
                            verify = true;
                            break;
                        case "You can not overdraw":
                            verify = false;
                            break;
                        default:
                            break;
                    }
                }
                return verify;

            }
            catch (SqlException ex)
            {
                Console.Out.WriteLine(ex.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }

            return verify;
        }
        #endregion

        #region Deposit
        /// <summary>
        /// Deposits money into the account
        /// </summary>
        /// <param name="account"></param>
        /// <param name="deposit"></param>
        /// <returns>True if successful and false if not</returns>
        public bool DepositAccount(AccountModel account, decimal deposit)
        {
            SqlCommand cmd = _sql.Execute("sp_Deposit");

            string? result;
            bool verify = false;

            cmd.Parameters.AddWithValue("AccountNumber", account.AccountNumber);
            cmd.Parameters.AddWithValue("Deposit", deposit);

            try
            {
                cmd.ExecuteNonQuery();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result = reader["Result"].ToString();
                    switch (result)
                    {
                        case "You have now deposited":
                            verify = true;
                            break;
                        case "Account does not exist":
                            verify = false;
                            break;
                        default:
                            break;
                    }
                }
                return verify;

            }
            catch (SqlException ex)
            {
                Console.Out.WriteLine(ex.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }

            return verify;
        }
        #endregion

    }
}
