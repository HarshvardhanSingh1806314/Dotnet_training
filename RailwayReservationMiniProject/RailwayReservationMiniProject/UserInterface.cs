using System.Configuration;
using System.Data.SqlClient;
using UserFunctionalities;

namespace RailwayReservationMiniProject
{
    public class UserInterface
    {
        public static void Main()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            Passenger passenger = new Passenger(sqlConnection, "PASSENGER");
            passenger.EnterPassengerArea();
        }
    }
}
