using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;


namespace StoredProcedure
{
    internal class Program
    {
        private static SqlConnection _databaseConnection;
        static void Main(string[] args)
        {
            ConnectToDatabase();
            RunStoredProcedureToInsertProduct();

            Console.ReadLine();
        }

        static void ConnectToDatabase()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
                _databaseConnection = new SqlConnection(connectionString);
                _databaseConnection.Open();
                if (_databaseConnection.State == ConnectionState.Open)
                {
                    Console.WriteLine("Database Connected Successfully");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void RunStoredProcedureToInsertProduct()
        {
            Console.Write("Enter Product Name: ");
            string productName = Console.ReadLine();
            Console.Write("Enter Product Price: ");
            float productPrice = float.Parse(Console.ReadLine());
            float discountedPrice = productPrice - (0.1f * productPrice);
            string query = "sp_InsertProducts";
            SqlCommand sqlCommand = new SqlCommand(query, _databaseConnection);
            sqlCommand.Parameters.AddWithValue("@productName", productName);
            sqlCommand.Parameters.AddWithValue("@price", productPrice);
            sqlCommand.Parameters.AddWithValue("@discountedPrice", discountedPrice);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            int result = sqlCommand.ExecuteNonQuery();
            if (result > 0)
            {
                Console.WriteLine("\n---------------------ProductDetails Table After Inserting a new Product---------------------");
                query = "select * from ProductDetails";
                sqlCommand = new SqlCommand(query, _databaseConnection);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"ProductID: {reader["productID"]}, ProductName: {reader["productName"]}, ProductPrice: {reader["price"]}, DiscountedPrice: {reader["discountedPrice"]}");
                }
            }
            else
            {
                Console.WriteLine("Product Insert Operation failed");
            }
        }
    }
}
