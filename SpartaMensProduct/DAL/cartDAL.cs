using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using SpartaMensProduct.Models;

namespace SpartaMensProduct.DAL
{

    /// <summary>
    /// Data access layer for the cart table conectiosns 
    /// </summary>
    public class cartDAL
    {

        string conString = ConfigurationManager.ConnectionStrings["mycrudconnectionstring"].ToString();


        public void AddToCart(int userId, int productId, int quantity)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand command = new SqlCommand("AddToCart", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@ProductId", productId);
                        command.Parameters.AddWithValue("@Quantity", quantity);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error Happend : " + ex.Message);
            }
        }




        public List<Cart> GetCartItems(int userId)
        {
            List<Cart> cartItems = new List<Cart>();
            try
            {

                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand command = new SqlCommand("GetCartItems", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserId", userId);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cartItems.Add(new Cart
                                {
                                    CartId = Convert.ToInt32(reader["CartId"]),
                                    ProductId = Convert.ToInt32(reader["ProductId"]),
                                    ProductName = reader["ProductName"].ToString(),
                                    Price = Convert.ToDecimal(reader["Price"]),
                                    Quantity = Convert.ToInt32(reader["Quantity"]),
                                    TotalPrice = Convert.ToDecimal(reader["TotalPrice"])
                                });
                            }
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error Happend : " + ex.Message);
            }

            return cartItems;
        }





        public void ClearCart(int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand command = new SqlCommand("ClearCart", connection))
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

                Console.WriteLine("Error Happend : " + ex.Message);
            }
        }

    }
}