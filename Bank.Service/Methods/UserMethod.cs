using Bank.Domain.Connections;
using Bank.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Service.Methods
{
    public class UserMethod
    {
        Users _user;

        public UserMethod()
        {
            _user = new Users();
        }

        public bool CreateUser(UserModel user)
            => _user.CreateUser(user);
        public bool DeleteUser(UserModel user)
            => _user.DeleteUser(user);
        public List<UserModel> GetAllUsers()
            => _user.GetAllUsers();
        public UserModel GetUserByMail(string mail)
            => _user.GetUserByMail(mail);
        public UserModel GetUserById(UserModel user)
            => _user.GetUserById(user);
        public UserModel UserLogin(UserModel user)
            => _user.UserLogin(user);
    }
}
