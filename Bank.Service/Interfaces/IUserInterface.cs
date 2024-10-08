﻿using Bank.Domain.Models;

namespace Bank.Service.Interfaces
{
    public interface IUserInterface
    {
        bool CreateUser(UserModel user);
        bool DeleteUser(UserModel user);
        List<UserModel> GetAllUsers();
        UserModel GetUserByMail(string mail);
        UserModel GetUserById(UserModel user);
        UserModel UserLogin(UserModel user);
    }
}
