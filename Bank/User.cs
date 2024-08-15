using Bank.Domain.Models;
using Bank.Service.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class User
    {
        UserMethod _user;
        public User()
        {
            _user = new UserMethod();
        }

        public UserModel UserLogin(UserModel user)
        {
            return _user.UserLogin(user);
        }

        public bool CreateUser(UserModel user)
        {
            return _user.CreateUser(user);
        }

        public UserModel GetUserByMail(string mail)
        {
            return _user.GetUserByMail(mail);
        }
    }
}
