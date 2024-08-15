using Bank.Domain.Models;
using System.ComponentModel.Design;

namespace Bank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo keyStartUp;
            UserModel userDetails = new UserModel();
            User _userObject = new User();
            string startUpMenu = @"
************************************
**       EUC Bank Project         **
************************************
**       Please Choose one        **
** L = Log-In                     **
** S = Sign up                    **
** X = Exit                       **
************************************
**       Press any shown key      **
************************************
                ";

            Console.WriteLine(startUpMenu);
            keyStartUp = Console.ReadKey();

            switch (keyStartUp.Key)
            {
                case ConsoleKey.L:
                    Console.WriteLine("Enter your email");
                    userDetails.Email = Console.ReadLine();
                    Console.WriteLine("Enter your password");
                    userDetails.Password = Console.ReadLine();
                    break;
                case ConsoleKey.S:
                    Console.WriteLine("Enter your name");
                    userDetails.Name = Console.ReadLine();
                    Console.WriteLine("Enter your age");
                    userDetails.Age = Int32.Parse(Console.ReadLine());
                    Console.WriteLine("Enter your email");
                    userDetails.Email = Console.ReadLine();
                    Console.WriteLine("Enter your password");
                    userDetails.Password = Console.ReadLine();
                    if (_userObject.CreateUser(userDetails))
                    {
                        string mail = userDetails.Email;
                        userDetails = _userObject.GetUserByMail(mail);

                        int id = userDetails.Id;

                        Menu(id);
                    }
                    else
                        break;
                    break;
                case ConsoleKey.X:
                    break;
                default:
                    break;
            }

            if (_userObject.UserLogin(userDetails) != null)
            {
                Menu(userDetails.Id);
            }
            else
            {

            }
        }
        public static void Menu(int userId)
        {
            bool loggedIn = false;
            Account _accountObject = new Account();
            AccountModel accountDetails = new AccountModel();
            ConsoleKeyInfo keyLoggedIn;
            string loggedInMenu = @$"
************************************
**       EUC Bank Project         **
************************************
**       Please Choose one        **
** M = Menu                       **
** C = Create account             **
** W = Withdraw                   **
** D = Deposist                   **
** I = Interest rate              **
** B = Show balance               **
** A = Show Accounts              **
** X = Exit/Logout                **
************************************
**       Press any shown key      **
************************************
                ";

            Console.WriteLine(loggedInMenu);
            keyLoggedIn = Console.ReadKey();
            do
            {
                switch (keyLoggedIn.Key)
                {
                    case ConsoleKey.M:
                        Console.Clear();
                        Console.WriteLine(loggedInMenu);
                        break;
                    case ConsoleKey.C:
                        Console.WriteLine("Enter your title");
                        accountDetails.TitleID = Int32.Parse(Console.ReadLine());
                        _accountObject.CreateAccount(accountDetails, userId);
                        break;
                    case ConsoleKey.W:
                        break;
                    case ConsoleKey.D:
                        break;
                    case ConsoleKey.I:
                        break;
                    case ConsoleKey.B:
                        break;
                    case ConsoleKey.A:
                        break;
                    case ConsoleKey.X:
                        loggedIn = false;
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            } while (loggedIn);
        }
    }
}
