using Bank.Domain.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Connections
{
    public class Users
    {
        private readonly Sql _sql;
        public Users()
        {
            _sql = new Sql();
        }

        #region Create User
        /// <summary>
        /// Creates User
        /// </summary>
        /// <param name="user"></param>
        /// <returns>True if successful, false if not</returns>
        public bool CreateUser(UserModel user)
        {
            SqlCommand cmd = _sql.Execute("sp_CreateUser");
            cmd.Parameters.AddWithValue("Mail", user.Email);
            cmd.Parameters.AddWithValue("Password", user.Password);
            cmd.Parameters.AddWithValue("Name", user.Name);
            cmd.Parameters.AddWithValue("Age", user.Age);

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

        #region Delete user
        /// <summary>
        /// Deletes user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>True if successful, false if not</returns>
        public bool DeleteUser(UserModel user)
        {
            SqlCommand cmd = _sql.Execute("sp_DeleteUser");
            cmd.Parameters.AddWithValue("UserId", user.Id);

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

        #region Get All Users
        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>Users if successful, null if not</returns>
        public List<UserModel> GetAllUsers()
        {
            SqlCommand cmd = _sql.Execute("sp_GetAllUsers");

            try
            {
                cmd.Connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                List<UserModel> user = new List<UserModel>();

                while (reader.Read())
                {
                    user.Add(new UserModel()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Age = reader.GetInt32(2),
                        Email = reader.GetString(3),
                        RegistrationDate = reader.GetDateTime(5)
                    });
                }
                return user;
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

        #region Get user by ID
        public UserModel GetUserById(UserModel user)
        {
            SqlCommand cmd = _sql.Execute("sp_GetUserByID");
            cmd.Parameters.AddWithValue("UserId", user.Id);

            try
            {
                cmd.Connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user.Name = reader.GetString(1);
                    user.Age = reader.GetInt32(2);
                    user.Email = reader.GetString(3);
                    user.RegistrationDate = reader.GetDateTime(5);
                }
                return user;
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

        #region User login
        /// <summary>
        /// Loggin in
        /// </summary>
        /// <param name="user"></param>
        /// <returns>User with id if successful, null if not</returns>
        public UserModel UserLogin(UserModel user)
        {
            SqlCommand cmd = _sql.Execute("sp_UserLogOn");

            string? result;

            cmd.Parameters.AddWithValue("Mail", user.Email);
            cmd.Parameters.AddWithValue("Password", user.Password);

            try
            {
                cmd.ExecuteNonQuery();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result = reader["Result"].ToString();
                    switch (result)
                    {
                        case "Login successful":
                            user.Id = reader.GetInt32(0);
                            user.Name = reader.GetString(1);
                            user.Age = reader.GetInt32(2);
                            break;
                        case "Login failed":
                            return null;
                        default:
                            break;
                    }
                }
                return user;

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
    }
}
