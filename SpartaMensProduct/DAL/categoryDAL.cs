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
    public class categoryDAL
    {

        string conString = ConfigurationManager.ConnectionStrings["mycrudconnectionstring"].ToString();


        public void AddCategory(Category category)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand command = new SqlCommand("sp_AddCategory", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                        command.Parameters.AddWithValue("@Description", category.Description);

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





        public List<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();
            try
            {

                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand command = new SqlCommand("sp_GetAllCategories", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categories.Add(new Category
                                {
                                    CategoryId = Convert.ToInt32(reader["CategoryId"]),
                                    CategoryName = reader["CategoryName"].ToString(),
                                    Description = reader["Description"].ToString()
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

            return categories;
        }



        public void DeleteCategory(int categoryId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand command = new SqlCommand("sp_DeleteCategory", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CategoryId", categoryId);

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