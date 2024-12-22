using System;
using System.Configuration;
using System.Data.SqlClient;
using UserFunctionalities;

namespace RailwayReservationMiniProject
{
    public class AdminInterface
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            Admin admin = new Admin(connection, "ADMIN");
            admin.EnterAdminArea();
        }
    }
}
