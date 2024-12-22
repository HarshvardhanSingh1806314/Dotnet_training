using CustomExceptions;
using System;
using System.Collections;
using System.Data.SqlClient;
using Utility;

namespace Authentication
{
    public class Authenticate
    {
        private readonly SqlConnection _connection;

        public Authenticate(SqlConnection connection)
        {
            _connection = connection;
        }

        public bool Register(
            string firstName, string lastName,
            string email, string password,
            string role, string phoneNumber
        )
        {
            try
            {
                // checking if anyone of the given inputs is null except lastName
                if(firstName == null)
                {
                    throw new NullValueException("First Name cannot be null");
                }
                if (email == null)
                {
                    throw new NullValueException("Email cannot be null");
                }
                if(password == null)
                {
                    throw new NullValueException("Password cannot be null");
                }
                if(role == null)
                {
                    throw new NullValueException("Role cannot be null");
                }
                if(phoneNumber == null)
                {
                    throw new NullValueException("Phone Number cannot be null");
                }

                // checking if the user with the given email already exist or not
                _connection.Open();
                string query = "select id from Users where email=@email";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@email", email);
                SqlDataReader sqlDataReader = command.ExecuteReader();
                if(sqlDataReader.HasRows)
                {
                    throw new UserAlreadyExistException($"User with email: {email} already exists");
                }
                sqlDataReader.Close();

                // encrypting the password and adding it to the database
                (string hashedPassword, string saltValue) = PasswordEncryption.HashPassword(password);
                string passwordId = GenerateIds.GeneratePasswordId(email, password);
                query = "insert into Passwords values(@passwordId, @hashedPassword, @saltValue)";
                command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@passwordId", passwordId);
                command.Parameters.AddWithValue("@hashedPassword", hashedPassword);
                command.Parameters.AddWithValue("@saltValue", saltValue);
                int result = command.ExecuteNonQuery();

                // checking if the password storage was successfull
                if(result == 0)
                {
                    throw new InsertionFailedException("Password Insertion Failed");
                }

                // getting role id for the user
                query = "select id from Roles where roleName = @roleName";
                command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@roleName", role.ToUpper());
                string roleId = null;
                sqlDataReader = command.ExecuteReader();
                if (sqlDataReader.Read()) 
                {
                    roleId = sqlDataReader[0].ToString();
                }
                sqlDataReader.Close();

                // generating user id for registering the user
                string generatedUserId = GenerateIds.GenerateUserId(role, phoneNumber);

                // inserting user information in the database
                query = "insert into Users values(@generatedUserId, @firstName, @lastName, @email, @password, @role, @phoneNumber)";
                command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@generatedUserId", generatedUserId);
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@email", email.ToUpper());
                command.Parameters.AddWithValue("@password", passwordId);
                command.Parameters.AddWithValue("@role", roleId);
                command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                result = command.ExecuteNonQuery();
                if(result > 0)
                {
                    _connection.Close();
                    return true;
                }
                else
                {
                    throw new Exception("User Registration Failed");
                }
            }
            catch(NullValueException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                _connection.Close();
            }

            return false;
        }

        public AuthenticationToken Login(string email, string password)
        {
            try
            {
                // checking if email or password is null
                if(email == null)
                {
                    throw new NullValueException("Email cannot be null");
                }
                if(password == null)
                {
                    throw new NullValueException("Password cannot be null");
                }

                // cheking if the user with the provided email exists or not
                _connection.Open();

                // fetching remaining user information
                string query = "select id, firstName, lastName, userPassword, userRole, phoneNumber from Users where email = @email";
                SqlCommand sqlCommand = new SqlCommand(query, _connection);
                sqlCommand.Parameters.AddWithValue("@email", email.ToUpper());
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                ArrayList userInfo;
                if (!sqlDataReader.HasRows)
                {
                    throw new UserNotFoundException($"User with email: {email} not found");
                }
                else
                {
                    userInfo = new ArrayList();
                    sqlDataReader.Read();
                    for(int i=0;i<sqlDataReader.FieldCount;i++)
                    {
                        userInfo.Add(sqlDataReader[i]);
                    }
                    sqlDataReader.Close();
                }

                // fetching the hashed user password
                query = "select password, salt from Passwords where id = @passwordId";
                sqlCommand = new SqlCommand(query, _connection);
                sqlCommand.Parameters.AddWithValue("@passwordId", userInfo[3]);
                sqlDataReader = sqlCommand.ExecuteReader();
                sqlDataReader.Read();
                string hashedPassword = sqlDataReader[0].ToString();
                string hashingSalt = sqlDataReader[1].ToString();
                sqlDataReader.Close();

                // checking if the entered password is correct or not if user exists
                if (PasswordEncryption.VerifyPassword(password, hashedPassword, hashingSalt))
                {
                    // fetching user role
                    query = "select roleName from Roles where id = @roleId";
                    sqlCommand = new SqlCommand(query, _connection);
                    sqlCommand.Parameters.AddWithValue("@roleId", userInfo[4]);
                    sqlDataReader = sqlCommand.ExecuteReader();
                    sqlDataReader.Read();
                    string userRole = sqlDataReader[0].ToString();

                    AuthenticationToken token = new AuthenticationToken
                    {
                        UserId = userInfo[0].ToString(),
                        FirstName = userInfo[1].ToString(),
                        LastName = userInfo[2].ToString(),
                        Role = userRole,
                        PhoneNumber = userInfo[5].ToString(),
                        CreatedAt = DateTime.Now,
                        ValidityDuration = 20
                    };

                    sqlDataReader.Close();
                    _connection.Close();

                    return token;                    
                }
                else
                {
                    throw new InvalidCredentialsException("Email or Password is incorrect");
                }
            }
            catch(InvalidCredentialsException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch(UserNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (NullValueException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally 
            { 
                _connection.Close();
            }
            return null;
        }

        public bool VerifyAccessToken(AuthenticationToken token)
        {
            try
            {
                if (token == null)
                {
                    throw new NullValueException("Authentication Token cannot be null");
                }

                double timeDuration = (DateTime.Now - token.CreatedAt).TotalMinutes;
                if(timeDuration < token.ValidityDuration)
                {
                    return true;
                }
            }
            catch (NullValueException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public void RefereshAccessToken(AuthenticationToken token)
        {
            token.CreatedAt = DateTime.Now;
        }
    }
}
