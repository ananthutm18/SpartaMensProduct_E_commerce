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
    /// Data access layer to manage product - databse functionalities
    /// </summary>
    public class productDAL
    {
        string conString = ConfigurationManager.ConnectionStrings["mycrudconnectionstring"].ToString();



        public void InsertProduct(Product product)
        {



            try
            {

                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (var command = new SqlCommand("sp_InsertProduct", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProductName", product.ProductName);
                        command.Parameters.AddWithValue("@Description", product.Description);
                        command.Parameters.AddWithValue("@Price", product.Price);
                        command.Parameters.AddWithValue("@Category", product.Category);
                        command.Parameters.AddWithValue("@Brand", product.Brand);
                        command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                        command.Parameters.AddWithValue("@IsActive", 1);
                        if (product.ImageData != null)
                        {
                            command.Parameters.Add("@Image", SqlDbType.VarBinary, -1).Value = product.ImageData;
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


        //get all products
        public IEnumerable<Product> GetAllProducts()
        {

            var products = new List<Product>();

            try
            {

                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (var command = new SqlCommand("sp_GetAllProducts", connection))
                    {


                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var product = new Product
                                {


                                    ProductId = Convert.ToInt32(reader["ProductId"]),
                                    ProductName = reader["ProductName"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    Price = Convert.ToDecimal(reader["Price"]),
                                    Category = reader["Category"].ToString(),
                                    Brand = reader["Brand"].ToString(),
                                    StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                                    ImageData = reader["Image"] != DBNull.Value ? (byte[])reader["Image"] : null

                                };

                                products.Add(product);
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

            return products;
        }

        //Get product details

        public Product GetProductById(int id)
        {
            Product product=null;
            try
            {

                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand command = new SqlCommand("GetProductDetails", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProductId", id);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                product = new Product
                                {
                                    ProductId = Convert.ToInt32(reader["ProductId"]),
                                    ProductName = reader["ProductName"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    Price = Convert.ToDecimal(reader["Price"]),
                                    Category = reader["Category"].ToString(),
                                    ImageData = reader["Image"] as byte[],
                                    StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
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
            return product;
        }

        //Update product   

        public void UpdateProduct(Product product)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (var command = new SqlCommand("sp_UpdateProduct", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProductId", product.ProductId);
                        command.Parameters.AddWithValue("@ProductName", product.ProductName);
                        command.Parameters.AddWithValue("@Description", product.Description);
                        command.Parameters.AddWithValue("@Price", product.Price);
                        command.Parameters.AddWithValue("@Category", product.Category);
                        command.Parameters.AddWithValue("@Brand", product.Brand);
                        command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                        command.Parameters.AddWithValue("@IsActive", product.IsActive);
                        if (product.ImageData != null)
                        {
                            command.Parameters.Add("@Image", SqlDbType.VarBinary, -1).Value = product.ImageData;
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

        //delete product
        public void DeleteProduct(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand command = new SqlCommand("sp_DeleteProductdata", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProductId", id);
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


        public int GetProductCount()
        {
           

            int productCount = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {

                    using (SqlCommand command = new SqlCommand("spCountProduct", connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();
                        productCount = (int)command.ExecuteScalar();
                        connection.Close();


                    }


                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
            }

            return productCount;

        }
        public List<Product> GetProductsByCategory(string Category)
        {
            var products = new List<Product>();
         

                using (var connection = new SqlConnection(conString))
                {
                    using (var command = new SqlCommand("GetProductsByCategory", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Category", Category);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var product = new Product
                                {
                                    ProductId = reader.GetInt32(0),
                                    ProductName = reader.GetString(1),
                                    Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    Price = reader.GetDecimal(3),
                                    Category = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    Brand = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    StockQuantity = reader.GetInt32(6),
                                    IsActive = reader.GetBoolean(7),
                                    ImageData = reader.IsDBNull(8) ? null : (byte[])reader[8]
                                };
                                products.Add(product);
                            }
                        }



                    }
                }
                      

            return products;
        }




        public List<Product> SearchProducts(string keyword)
        {
            List<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("SearchProducts", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Keyword", keyword);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                     var product = new Product
                     {
                         ProductId = reader.GetInt32(0),
                         ProductName = reader.GetString(1),
                         Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                         Price = reader.GetDecimal(3),
                         Category = reader.IsDBNull(4) ? null : reader.GetString(4),
                         Brand = reader.IsDBNull(5) ? null : reader.GetString(5),
                         StockQuantity = reader.GetInt32(6),
                         IsActive = reader.GetBoolean(7),
                         ImageData = reader.IsDBNull(8) ? null : (byte[])reader[8]
                     };
                    products.Add(product);
                }
                connection.Close();
            }

            return products;
        }


    }
}