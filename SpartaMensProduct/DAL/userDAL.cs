using SpartaMensProduct.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

using BCrypt.Net;

namespace SpartaMensProduct.DAL
{

    /// <summary>
    /// Data access layer for the user dtaabse connection
    /// </summary>
    public class userDAL
    {
        string conString = ConfigurationManager.ConnectionStrings["mycrudconnectionstring"].ToString();

        //Register User

        public void CreateUser(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand command = new SqlCommand("sp_RegisterUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Hashing  the password before storing it 
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

                        command.Parameters.AddWithValue("@FirstName", user.FirstName);
                        command.Parameters.AddWithValue("@LastName", user.LastName);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                        command.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@IsAdmin", 0);
                        command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                        command.Parameters.AddWithValue("@Address", user.Address);
                        command.Parameters.AddWithValue("@City", user.City);
                        command.Parameters.AddWithValue("@State", user.State);
                        if (user.ImageData != null)
                        {
                            command.Parameters.Add("@Image", SqlDbType.VarBinary, -1).Value = user.ImageData;
                        }
                        else
                        {
                            command.Parameters.Add("@Image", SqlDbType.VarBinary, -1).Value = DBNull.Value;
                        }
                        command.Parameters.AddWithValue("@CreatedAt", user.CreatedAt);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
            }
        }

        public dynamic ValidateUser(string email, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (var command = new SqlCommand("sp_ValidateUserChecks", connection))
                    {
                        command.CommandTimeout = 180; // Timeout in seconds

                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Email", email);

                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var storedPasswordHash = reader["PasswordHash"]?.ToString();

                                System.Diagnostics.Debug.WriteLine("Stored Password Hashed value: " + storedPasswordHash);

                                if (!string.IsNullOrEmpty(storedPasswordHash) && storedPasswordHash.Length == 60)
                                {
                                    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, storedPasswordHash);

                                    if (isPasswordValid)
                                    {
                                        return new
                                        {
                                            UserId = reader["UserId"],
                                            FirstName = reader["FirstName"],
                                            LastName = reader["LastName"],
                                            Email = reader["Email"],
                                            IsAdmin = reader["IsAdmin"]
                                        };
                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine("Password verificationsssaas failed.");
                                    }
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine("Invalid passwordsdds hash length.");
                                }
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("User not found---------.");
                            }

                            connection.Close();
                        }
                    }
                   // return null;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
            }
            return null;
        }

        public User GetUserById(int userId)
        {
            User user = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand command = new SqlCommand("sp_GetUserDataById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserId", userId);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User
                                {
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    ImageData = reader["Image"] as byte[],

                                };
                            }
                        }
                        connection.Close();
                    }
                }
               
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
            }


            return user;

        }



        //Get all users=======================================

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            try
            {

                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand command = new SqlCommand("sp_GetAllUsers", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User user = new User
                                {
                                    UserId = (int)reader["UserId"],
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    IsAdmin = (bool)reader["IsAdmin"],
                                    DateOfBirth = (DateTime)reader["DateOfBirth"],
                                    Address = reader["Address"].ToString(),
                                    City = reader["City"].ToString(),
                                    State = reader["State"].ToString(),
                                    PhoneNumber = reader["PhoneNumber"].ToString(),
                                    CreatedAt = (DateTime)reader["CreatedAt"]
                                };

                                if (reader["Image"] != DBNull.Value)
                                {
                                    user.ImageData = (byte[])reader["Image"];
                                }

                                users.Add(user);
                            }
                        }
                        connection.Close();
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
            }


            return users;
        }



        public void DeleteUser(int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand command = new SqlCommand("sp_DeleteUserAndRelatedData", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserId", userId);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
            }
        }



        public void EditUser(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand command = new SqlCommand("sp_UpdateUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);


                        command.Parameters.AddWithValue("@UserId", user.UserId); // Assuming Id is the unique identifier for the user
                        command.Parameters.AddWithValue("@FirstName", user.FirstName);
                        command.Parameters.AddWithValue("@LastName", user.LastName);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                        command.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                        command.Parameters.AddWithValue("@Address", user.Address);
                        command.Parameters.AddWithValue("@City", user.City);
                        command.Parameters.AddWithValue("@State", user.State);

                        if (user.ImageData != null)
                        {
                            command.Parameters.Add("@Image", SqlDbType.VarBinary, -1).Value = user.ImageData;
                        }
                        else
                        {
                            command.Parameters.Add("@Image", SqlDbType.VarBinary, -1).Value = DBNull.Value;
                        }


                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
            }
        }




        public void Logout(HttpSessionStateBase session)
        {
            try
            {
                session.Clear();
                session.Abandon();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
            }
        }
    }

}