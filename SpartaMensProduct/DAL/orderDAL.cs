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
    public class orderDAL
    {

        string conString = ConfigurationManager.ConnectionStrings["mycrudconnectionstring"].ToString();


        public int CreateOrder(int userId, decimal totalAmount, string paymentMethod)
        {
           
            int orderId=0;

            try
            {


                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand cmd = new SqlCommand("CreateOrders", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                        cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);

                        SqlParameter outputOrderId = new SqlParameter("@OrderId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputOrderId);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                        orderId = (int)outputOrderId.Value;
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
            }
            return orderId;
        }

        //Add order item

        public void AddOrderItem(int orderId, int productId, int quantity, decimal unitPrice)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    using (SqlCommand cmd = new SqlCommand("AddOrderItem", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OrderId", orderId);
                        cmd.Parameters.AddWithValue("@ProductId", productId);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.Parameters.AddWithValue("@UnitPrice", unitPrice);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
            }
        }
        public IEnumerable<Order> GetOrdersByUserId(int userId)
        {
            var orders = new List<Order>();

            try
            {

                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetOrdersByUserId", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var order = new Order
                                {
                                    OrderId = reader.GetInt32(0),
                                    TotalAmount = reader.GetDecimal(1),
                                    PaymentMethod = reader.GetString(2),
                                    OrderDate = reader.GetDateTime(3),
                                    Status = reader.GetString(4),
                                    OrderItems = new List<OrderItem>
                                {
                                    new OrderItem
                                    {
                                        ProductId = reader.GetInt32(5),
                                        Quantity = reader.GetInt32(6),
                                        ProductName = reader.GetString(7),
                                        Price = reader.GetDecimal(8)
                                    }
                                }
                                };

                                orders.Add(order);
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

            return orders;
        }


        public IEnumerable<Order> GetAllOrders()
        {
            var orders = new List<Order>();

            try
            {

                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand command = new SqlCommand("sp_GetAllOrders", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var order = new Order
                                {
                                    OrderId = reader.GetInt32(0),
                                    UserId = reader.GetInt32(1),
                                    TotalAmount = reader.GetDecimal(2),
                                    PaymentMethod = reader.GetString(3),
                                    OrderDate = reader.GetDateTime(4),
                                    Status = reader.GetString(5),
                                    User = new User
                                    {
                                        FirstName = reader.GetString(6),
                                        LastName = reader.GetString(7),
                                        Email = reader.GetString(8),
                                        Address = reader.GetString(9),
                                        PhoneNumber = reader.GetString(10),
                                    }
                                };
                                orders.Add(order);
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

            return orders;
        }

        public void UpdateOrderStatus(int orderId, string status)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand command = new SqlCommand("sp_UpdateOrderStatus", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@OrderId", orderId);
                        command.Parameters.AddWithValue("@Status", status);

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

        public int GetOrderCount()
        {
            int orderCount = 0;

            try
            {

                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand command = new SqlCommand("sp_GetOrderCount", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        orderCount = (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
            }

            return orderCount;
        }


        public IEnumerable<Order> GetAllOrdersLatest()
        {
            var orders = new List<Order>();

            using (SqlConnection connection = new SqlConnection(conString))
            {
                using (SqlCommand command = new SqlCommand("sp_GetAllOrdersdata", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var orderId = reader.GetInt32(0);
                            var order = orders.FirstOrDefault(o => o.OrderId == orderId);
                            if (order == null)
                            {
                                order = new Order
                                {
                                    OrderId = orderId,
                                    UserId = reader.GetInt32(1),
                                    TotalAmount = reader.GetDecimal(2),
                                    PaymentMethod = reader.GetString(3),
                                    OrderDate = reader.GetDateTime(4),
                                    Status = reader.GetString(5),
                                    User = new User
                                    {
                                        FirstName = reader.GetString(6),
                                        LastName = reader.GetString(7),
                                        Email = reader.GetString(8),
                                        Address = reader.GetString(9),
                                        PhoneNumber = reader.GetString(10)
                                    },
                                    OrderItems = new List<OrderItem>()
                                };
                                orders.Add(order);
                            }

                            var orderItem = new OrderItem
                            {
                                ProductId = reader.GetInt32(11),
                                ProductName = reader.GetString(12)
                            };

                            order.OrderItems.Add(orderItem);
                        }
                    }

                    connection.Close();
                }
            }

            return orders;
        }

    }
}