using Bank.Domain.Models;
using System.ComponentModel.Design;

namespace Bank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StartUpMenu();
        }
        public static void StartUpMenu()
        {
            ConsoleKeyInfo keyStartUp;
            UserModel userDetails = new UserModel();
            User userObject = new User();
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
            keyStartUp = Console.ReadKey(true);

            switch (keyStartUp.Key)
            {
                case ConsoleKey.L:
                    Console.WriteLine("Enter your email");
                    userDetails.Email = Console.ReadLine();
                    Console.WriteLine("Enter your password");
                    userDetails.Password = Console.ReadLine();
                    if (userObject.UserLogin(userDetails) != null)
                    {
                        string mail = userDetails.Email;
                        userDetails = userObject.GetUserByMail(mail);

                        int id = userDetails.Id;

                        Menu(id);
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Login failed");
                    }
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
                    if (userObject.CreateUser(userDetails))
                    {
                        string mail = userDetails.Email;
                        userDetails = userObject.GetUserByMail(mail);

                        int id = userDetails.Id;

                        Menu(id);
                    }
                    break;
                case ConsoleKey.X:
                    Environment.Exit(0);
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Wrong input try again\n");
                    GoBack();
                    StartUpMenu();
                    break;
            }
        }
        public static void Menu(int userId)
        {
            bool loggedIn = false;
            UserModel userDetails = new UserModel();
            User userObject = new User();
            Account accountObject = new Account();
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
** U = Show your details          **
** A = Show Accounts              **
** X = Exit/Logout                **
************************************
**       Press any shown key      **
************************************
                ";

            Console.WriteLine(loggedInMenu);
            keyLoggedIn = Console.ReadKey(true);


            do
            {
                switch (keyLoggedIn.Key)
                {
                    case ConsoleKey.M:
                        Console.Clear();
                        Console.WriteLine(loggedInMenu);
                        break;
                    case ConsoleKey.C:
                        Console.WriteLine(@"Account Types:
              1. Checking Account   |   2. Consumer Account   |   3. Savings Account
                        ");
                        Console.WriteLine("Enter your account type: ");
                        int titleID = Int32.Parse(Console.ReadLine());
                        accountDetails.TitleID = titleID - 1;
                        accountObject.CreateAccount(accountDetails, userId);
                        break;
                    case ConsoleKey.W:
                        break;
                    case ConsoleKey.D:
                        break;
                    case ConsoleKey.I:
                        break;
                    case ConsoleKey.U:
                        userDetails = userObject.GetUserById(userId);
                        Console.WriteLine($"User Details:\nName:{userDetails.Name}\nAge:{userDetails.Age}\nE-mail:{userDetails.Email}");
                        break;
                    case ConsoleKey.A:
                        Console.WriteLine("Accounts:");
                        string name;
                        foreach (AccountModel account in accountObject.Accounts(userId))
                        {
                            name = ((AccountType.Types)account.TitleID).ToString().Replace("_", " ");
                            Console.WriteLine($"| Account Number: {account.AccountNumber} | Account type: {name} | Account balance: {account.Balance} | Account Interest rate: {account.InterestRate}");
                        }
                        break;
                    case ConsoleKey.X:
                        loggedIn = false;
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Wrong input try again\n");
                        break;
                }
                GoBack();
                Console.Clear();
                Menu(userId);
            } while (loggedIn);
        }
        public static void GoBack()
        {
            Console.WriteLine("\nPress any key to continue");
            Console.ReadKey();
        }
    }
}
