using Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserFunctionalities
{
    public class User
    {
        protected readonly SqlConnection _connection;
        protected AuthenticationToken _token;
        protected readonly Authenticate _auth;
        protected readonly string _role;

        public User(SqlConnection connection, string role)
        {
            _connection = connection;
            _auth = new Authenticate(connection);
            _role = role;
        }

        protected void Login()
        {
            Console.Clear();

            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            _token = _auth.Login(email, password);
        }

        protected bool Register()
        {
            Console.Clear();

            // taking firstName as input
            Console.Write("Enter FirstName: ");
            string firstName = Console.ReadLine();

            Console.Write("Enter LastName: ");
            string lastName = Console.ReadLine();

            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            Console.Write("Enter Phone Number: ");
            string phoneNumber = Console.ReadLine();

            // registering new user
            return _auth.Register(
                firstName, lastName, email, password, _role, phoneNumber
            );
        }

        protected bool ExecuteQuery(string query, Dictionary<string, object> parameters)
        {
            try
            {
                _connection.Open();
                SqlCommand sqlCommand = new SqlCommand(query, _connection);
                foreach (KeyValuePair<string, object> param in parameters)
                {
                    sqlCommand.Parameters.AddWithValue(param.Key.Trim(), param.Value);
                }
                int noOfRowsEffected = sqlCommand.ExecuteNonQuery();
                return noOfRowsEffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                _connection.Close();
            }

            return false;
        }
    }
}
