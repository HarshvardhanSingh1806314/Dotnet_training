using CustomExceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using Utility;

namespace UserFunctionalities
{ 
    public class Admin : User
    {
        public Admin(SqlConnection connection, string role) : base(connection, role) { }

        private string ConstructUpdateQuery(string tableName, Dictionary<string, object> parameters)
        {
            string query = $"update {tableName} set ";
            foreach(KeyValuePair<string, object> param in parameters)
            {
                query += param.Key.Substring(1, param.Key.Length - 1) + " = " + param.Key + ", ";
            }
            return query.Trim().Substring(0, query.Length - 2);
        }

        // INSERT version of Train Status
        private bool IUDTrainStatus(string trainStatus, out string generatedTrainStatusId)
        {
            generatedTrainStatusId = GenerateIds.GenerateTrainStatusId(trainStatus.ToUpper());
            string query = "insert into TrainStatus values(@generatedTrainStatusId, @trainStatus)";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@generatedTrainStatusId", generatedTrainStatusId.Trim() },
                { "@trainStatus", trainStatus.ToUpper().Trim() }
            };
            return ExecuteQuery(query, parameters);
        }

        // UPDATE version of Train Status
        private bool IUDTrainStatus(string trainStatusId, string trainStatus)
        {
            string query = "update TrainStatus set statusName = @trainStatus where id = @trainStatusId";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@trainStatus", trainStatus.ToUpper().Trim()},
                {"@trainStatusId", trainStatusId.Trim() }
            };
            return ExecuteQuery(query, parameters);
        }

        // DELETE version of TrainStatus
        private bool IUDTrainStatus(string trainStatusId)
        {
            string query = "delete from TrainStatus where id = @trainStatusId";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@trainStatusId", trainStatusId.Trim()}
            };
            return ExecuteQuery(query, parameters);
        }

        // INSERT version of Trains
        private bool IUDTrains(
            string trainName, int availableSeatsInA1, int availableSeatsInA2,
            int availableSeatsInA3, int totalSeatsInA1, int totalSeatsInA2, int totalSeatsInA3,
            string trainStatus, out int trainNo
        )
        {
            try
            {
                if (availableSeatsInA1 <= 0 || availableSeatsInA2 <= 0 || availableSeatsInA3 <= 0)
                {
                    throw new NullValueException("Available Seats cannot be less than or equal to 0");
                }
                if (totalSeatsInA1 <= 0 || totalSeatsInA2 <= 0 || totalSeatsInA3 <= 0)
                {
                    throw new NullValueException("Total Seats cannot be less than or equal to 0");
                }
                if (availableSeatsInA1 > totalSeatsInA1 || availableSeatsInA2 > totalSeatsInA2 || availableSeatsInA3 > totalSeatsInA3)
                {
                    throw new InvalidValueException("Available Seats Cannot be more than Total Seats");
                }

                // getting all the train numbers in the database to generate a unique train no
                string query = "select trainNo from Trains";
                _connection.Open();
                SqlCommand sqlCommand = new SqlCommand(query, _connection);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                ArrayList trainNoList = new ArrayList();
                while (sqlDataReader.Read())
                {
                    trainNoList.Add(sqlDataReader[0]);
                }
                sqlDataReader.Close();

                // getting the train status id
                //query = "select id from TrainStatus where statusName = @statusName";
                //sqlCommand = new SqlCommand(query, _connection);
                //sqlCommand.Parameters.AddWithValue("@statusName", trainStatus.ToUpper());
                //sqlDataReader = sqlCommand.ExecuteReader();
                //string trainStatusId = null;
                //while(sqlDataReader.Read())
                //{
                //    trainStatusId = sqlDataReader[0].ToString();
                //}
                _connection.Close();
                trainNo = GenerateIds.GenerateTrainNo(trainNoList);

                Dictionary<string, int> berths = new Dictionary<string, int>
                {
                    {"A1", totalSeatsInA1 },
                    {"A2", totalSeatsInA2 },
                    {"A3", totalSeatsInA3 }
                };

                query = "insert into Trains values(@trainNo, @trainName, @trainStatus, @availableSeatsInA1, @availableSeatsInA2, " +
                    "@availableSeatsInA3, @totalSeatsInA1, @totalSeatsInA2, @totalSeatsInA3)";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    {"@trainNo", trainNo},
                    {"@trainName", trainName.Trim() },
                    {"@trainStatus", trainStatus.Trim() },
                    {"@availableSeatsInA1", availableSeatsInA1},
                    {"@availableSeatsInA2", availableSeatsInA2 },
                    {"@availableSeatsInA3", availableSeatsInA3 },
                    {"@totalSeatsInA1", totalSeatsInA1},
                    {"@totalSeatsInA2", totalSeatsInA2 },
                    {"@totalSeatsInA3", totalSeatsInA3 }
                };
                
                //return ExecuteQuery(query, parameters) && IUDBerth(berths, trainNo);
                return ExecuteQuery(query, parameters);
            }
            catch (NullValueException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            trainNo = 0;
            return false;
        }

        // UPDATE version of Trains
        private bool UpdateTrainStatus(int trainNo, out string updatedTrainStatus)
        {
            try
            {
                _connection.Open();

                // fetching the current status of train
                string query = "select trainStatus from Trains where trainNo = @trainNo";
                SqlCommand sqlCommand = new SqlCommand(query, _connection);
                sqlCommand.Parameters.AddWithValue("@trainNo", trainNo);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                string currentTrainStatus = null;
                while(reader.Read())
                {
                    currentTrainStatus = reader[0].ToString();
                }
                sqlCommand.Parameters.Clear();
                reader.Close();

                // fetching the opposite state of the current state of train
                query = "select id from TrainStatus where id != @trainStatusId";
                sqlCommand.CommandText = query;
                sqlCommand.Parameters.AddWithValue("@trainStatusId", currentTrainStatus);
                reader = sqlCommand.ExecuteReader();
                updatedTrainStatus = null;
                while(reader.Read())
                {
                    updatedTrainStatus = reader[0].ToString();
                }
                sqlCommand.Parameters.Clear();
                reader.Close();
                _connection.Close();

                // updating train status
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    {"@trainStatus", updatedTrainStatus }
                };
                query = ConstructUpdateQuery("Trains", parameters) + " where trainNo = @trainNo";
                parameters.Add("@trainNo", trainNo);
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _connection.Close();
            }

            updatedTrainStatus = null;
            return false;
        }

        // DELETE version of Trains
        private bool IUDTrains(int trainNo)
        {
            try
            {
                string query = "delete from Trains where trainNo = @trainNo";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    {"@trainNo", trainNo}
                };
                if(ExecuteQuery(query, parameters))
                {
                    _connection.Open();
                    query = "sp_DropBerthTablesOfDeletedTrain";
                    SqlCommand sqlCommand = new SqlCommand(query, _connection);
                    sqlCommand.Parameters.AddWithValue("@trainNo", trainNo);
                    SqlParameter outputParameter = new SqlParameter("@dropped", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    sqlCommand.Parameters.Add(outputParameter);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.ExecuteScalar();
                    if(bool.Parse(sqlCommand.Parameters["@dropped"].Value.ToString()))
                    {
                        return true;
                    }
                }
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

        // INSERT version of Distance
        private bool IUDDistance(string boardingStation, string destination, float distance, int trainNo, out string distanceId)
        {
            try
            {
                distanceId = GenerateIds.GenerateDistanceId(boardingStation, destination, distance);
                string query = "insert into Distance values(@distanceId, @boardingStation, @destination, @distance, @trainNo)";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    {"@distanceId", distanceId },
                    {"@boardingStation", boardingStation.Trim() },
                    {"@destination", destination.Trim() },
                    {"@distance", distance},
                    {"@trainNo", trainNo}
                };
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            distanceId = null;
            return false;
        }

        // UPDATE version of Distance
        private bool IUDDistance(string distanceId, Dictionary<string, object> columnsToUpdate)
        {
            try
            {
                string query = ConstructUpdateQuery("Distance", columnsToUpdate) + " where id = @distanceId";
                columnsToUpdate.Add("@distanceId", distanceId);
                return ExecuteQuery(query, columnsToUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }

        // DELETE version of Distance
        private bool IUDDistance(string distanceId)
        {
            try
            {
                string query = "delete from Distance where id = @distanceId";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@distanceId", distanceId }
                };
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }

        // INSERT version of BerthStatus
        private bool IUDBerthStatus(string statusName, out string berthStatusId)
        {
            try
            {
                berthStatusId = GenerateIds.GenerateBerthStatusId(statusName);
                string query = "insert into BerthStatus values(@berthStatusId, @statusName)";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    {"@berthStatusId", berthStatusId },
                    {"@statusName", statusName.ToUpper()}
                };
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            berthStatusId = null;
            return false;
        }

        // UPDATE version of BerthStatus
        private bool IUDBerthStatus(string berthStatusId, string statusName)
        {
            try
            {
                string query = "update BerthStatus set statusName = @statusName where id = @berthStatusId";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    {"@statusName", statusName.ToUpper()},
                    {"@berthStatusId", berthStatusId}
                };
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }

        // DELETE version of BerthStatus
        private bool IUDBerthStatus(string berthStatusId)
        {
            try
            {
                string query = "delete from BerthStatus where id = @berthStatusId";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    {"@berthStatusId", berthStatusId }
                };
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return false;
        }

        // INSERT version of BerthClass
        private bool IUDBerthClass(string berthClass, float pricePerKm, out string berthClassId)
        {
            try
            {
                berthClassId = GenerateIds.GenerateBerthClassId(berthClass, pricePerKm);
                string query = "insert into BerthClass values(@berthClassId, @berthClass, @pricePerKm)";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    {"@berthClassId", berthClassId },
                    {"@berthClass", berthClass.ToUpper() },
                    {"@pricePerKm", pricePerKm}
                };
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            berthClassId = null;
            return false;
        }

        // UPDATE version of BerthClass
        private bool IUDBerthClass(string berthClassId, Dictionary<string, object> columnsToUpdate)
        {
            try
            {
                string query = ConstructUpdateQuery("BerthClass", columnsToUpdate) + " where id = @berthClassId";
                columnsToUpdate.Add("@berthClassId", berthClassId);
                return ExecuteQuery(query, columnsToUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return false;
        }

        // DELETE version of BerthClass
        private bool IUDBerthClass(string berthClassId)
        {
            try
            {
                string query = "delete from BerthClass where id = @berthClassId";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    {"@berthClassId", berthClassId},
                };
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return false;
        }

        // INSERT version of Roles
        private bool IUDRoles(string roleName, out string generatedRoleId)
        {
            try
            {
                generatedRoleId = GenerateIds.GenerateRolesId(roleName);
                string query = "insert into Roles values(@generatedRoleId, @roleName)";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    {"@generatedRoleId", generatedRoleId},
                    {"@roleName", roleName},
                };
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            generatedRoleId = null;
            return false;
        }

        // UPDATE version of Roles
        private bool IUDRoles(string roleId, Dictionary<string, object> columnsToUpdate)
        {
            try
            {
                string query = ConstructUpdateQuery("Roles", columnsToUpdate) + " where id = @roleId";
                columnsToUpdate.Add("@roleId", roleId);
                columnsToUpdate["@roleName"] = columnsToUpdate["@roleName"].ToString().ToUpper();
                return ExecuteQuery(query, columnsToUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return false;
        }

        // DELETE version of Roles
        private bool IUDRoles(string roleId)
        {
            try
            {
                string query = "delete from Roles where id = @roleId";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    {"@roleId", roleId }
                };
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return false;
        }

        // INSERT version of TicketStatus
        private bool IUDTicketStatus(string ticketStatus, out string generatedTicketStatusId)
        {
            try
            {
                generatedTicketStatusId = GenerateIds.GenerateTicketStatusId(ticketStatus);
                string query = "insert into TicketStatus values(@generatedTicketStatusId, @ticketStatus)";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    {"@generatedTicketStatusId", generatedTicketStatusId },
                    {"@ticketStatus", ticketStatus.ToUpper() }
                };
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            generatedTicketStatusId = null;
            return false;
        }

        // UPDATE version of TicketStatus
        private bool IUDTicketStatus(string ticketStatusId, Dictionary<string, object> columnsToUpdate)
        {
            try
            {
                string query = ConstructUpdateQuery("TicketStatus", columnsToUpdate) + " where id = @ticketStatusId";
                columnsToUpdate.Add("@ticketStatusId", ticketStatusId);
                columnsToUpdate["@statusName"] = columnsToUpdate["@statusName"].ToString().ToUpper();
                return ExecuteQuery(query, columnsToUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return false;
        }

        // DELETE version of TicketStatus
        private bool IUDTicketStatus(string ticketStatusId)
        {
            try
            {
                string query = "delete from TicketStatus where id = @ticketStatusId";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    {"@ticketStatusId", ticketStatusId},
                };
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return false;
        }

        private void DisplayUserInterface()
        {
            Console.Clear();
            Console.WriteLine("Press 1 to INSERT/UPDATE/DELETE Train Status Values");
            Console.WriteLine("Press 2 to INSERT/UPDATE/DELETE Trains");
            Console.WriteLine("Press 3 to INSERT/UPDATE/DELETE Distance Values");
            Console.WriteLine("Press 4 to INSERT/UPDATE/DELETE Berth Status Values");
            Console.WriteLine("Press 5 to INSERT/UPDATE/DELETE Berth Class Values");
            Console.WriteLine("Press 6 to INSERT/UPDATE/DELETE Ticket Status Values");
            Console.WriteLine("Press 7 to INSERT/UPDATE/DELETE User Roles Values");
            Console.WriteLine("Press 8 to Exit");
        }

        private void DisplayTableData(string tableName)
        {
            try
            {
                _connection.Open();
                string query = $"select * from {tableName}";
                SqlCommand sqlCommand = new SqlCommand(query, _connection);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                for(int i=0;i<sqlDataReader.FieldCount;i++)
                {
                    Console.Write($"{sqlDataReader.GetName(i)}     ");
                }
                Console.WriteLine();
                while(sqlDataReader.Read())
                {
                    for(int i=0;i<sqlDataReader.FieldCount;i++)
                    {
                        Console.Write($"{sqlDataReader[i]}         ");
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                _connection.Close();
            }
        }

        private void AuthenticateAdmin()
        {
            int choice = 0;
            while (choice != 3)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("Welcome to ADMIN area");
                    Console.WriteLine("Press 1 to Login");
                    Console.WriteLine("Press 2 to Register");
                    Console.WriteLine("Press 3 to Exit");
                    Console.Write("Enter your choice: ");
                    choice = int.Parse(Console.ReadLine().Trim());
                    switch (choice)
                    {
                        case 1:
                            Login();
                            if (_token != null)
                            {
                                if(_token.Role == "PASSENGER")
                                {
                                    throw new UserNotAuthorizedException("As PASSENGER you are not authorized to access ADMIN funcionalities");
                                }
                                choice = 3;   
                            }
                            break;
                        case 2:
                            bool isUserRegistered = Register();
                            if (isUserRegistered)
                            {
                                Console.WriteLine("You are being redirected to login page");
                                choice = 1;
                            }
                            else
                            {
                                Console.WriteLine("Try Aagin to register");
                            }
                            Thread.Sleep(1000);
                            break;
                        case 3:
                            Console.WriteLine("Exiting.");
                            Thread.Sleep(1000);
                            break;
                        default:
                            Console.WriteLine("Invalid Choice");
                            Thread.Sleep(1000);
                            break;
                    }
                }
                catch(FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                    Thread.Sleep(1000);
                }
                catch (UserNotAuthorizedException ex)
                {
                    _token = null;
                    choice = 3;
                    Console.WriteLine(ex.Message);
                    Thread.Sleep(1000);
                }
            }
        }

        public void EnterAdminArea()
        {
            // Authenticating User
            AuthenticateAdmin();

            if (_token != null)
            {
                int operationChoice = 0;
                while (operationChoice != 8)
                {
                    DisplayUserInterface();
                    try
                    {
                        operationChoice = int.Parse(Console.ReadLine());
                        switch (operationChoice)
                        {
                            case 1:
                                int IUDChoice = 0;
                                while (IUDChoice != 4)
                                {
                                    Console.Clear();
                                    DisplayTableData("TrainStatus");
                                    Console.WriteLine("\nPress 1 to INSERT new Train Status value");
                                    Console.WriteLine("Press 2 to UPDATE a Train Status value");
                                    Console.WriteLine("Press 3 to DELETE a Train Status value");
                                    Console.WriteLine("Press 4 to EXIT");
                                    Console.Write("Enter your choice: ");
                                    IUDChoice = int.Parse(Console.ReadLine().Trim());
                                    switch (IUDChoice)
                                    {
                                        case 1:
                                            // calling IUDTrainStatus insert version function
                                            Console.Clear();
                                            Console.Write("Enter New TrainStatus Value: ");
                                            string trainStatus = Console.ReadLine();
                                            if (!_auth.VerifyAccessToken(_token))
                                            {
                                                _auth.RefereshAccessToken(_token);
                                            }
                                            bool result = IUDTrainStatus(trainStatus, out string trainStatusId);
                                            if (result)
                                            {
                                                Console.WriteLine(trainStatusId);
                                            }
                                            Thread.Sleep(1000);
                                            break;
                                        case 2:
                                            // calling the IUDTrainStatus Update version function
                                            Console.Clear();
                                            DisplayTableData("TrainStatus");
                                            Console.Write("Enter New TrainStatus Value: ");
                                            trainStatus = Console.ReadLine();
                                            Console.Write("Enter TrainStatus Id: ");
                                            trainStatusId = Console.ReadLine();
                                            if (!_auth.VerifyAccessToken(_token))
                                            {
                                                _auth.RefereshAccessToken(_token);
                                            }
                                            result = IUDTrainStatus(trainStatusId, trainStatus);
                                            if (result)
                                            {
                                                Console.WriteLine("TrainStatus updated successfully");
                                            }
                                            else
                                            {
                                                Console.WriteLine("TrainStatus Updation Failed");
                                            }
                                            Thread.Sleep(1000);
                                            break;
                                        case 3:
                                            // call the IUDTrainStatus Delete version function
                                            Console.Clear();
                                            DisplayTableData("TrainStatus");
                                            Console.Write("Enter TrainStatus Id: ");
                                            trainStatusId = Console.ReadLine();
                                            if (!_auth.VerifyAccessToken(_token))
                                            {
                                                _auth.RefereshAccessToken(_token);
                                            }
                                            result = IUDTrainStatus(trainStatusId);
                                            if (result)
                                            {
                                                Console.WriteLine($"TrainStatus Id {trainStatusId} deleted successfully");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Deletion Failed");
                                            }
                                            Thread.Sleep(1000);
                                            break;
                                        case 4:
                                            Console.WriteLine("Exiting.");
                                            Thread.Sleep(1000);
                                            break;
                                        default:
                                            Console.WriteLine("Invalid Choice");
                                            break;
                                    }
                                }
                                break;
                            case 2:
                                IUDChoice = 0;
                                while (IUDChoice != 4)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Press 1 to INSERT a Train value");
                                    Console.WriteLine("Press 2 to UPDATE Train Status");
                                    Console.WriteLine("Press 3 to DELETE a Train");
                                    Console.WriteLine("Press 4 to Exit");
                                    IUDChoice = int.Parse(Console.ReadLine());
                                    switch (IUDChoice)
                                    {
                                        case 1:
                                            Console.Clear();
                                            Console.WriteLine("Displaying All Trains");
                                            DisplayTableData("Trains");
                                            Console.WriteLine();
                                            Console.WriteLine("Displaying All Available Train Status With IDs");
                                            DisplayTableData("TrainStatus");
                                            Console.WriteLine();
                                            Console.Write("Enter Train Name: ");
                                            string trainName = Console.ReadLine();
                                            Console.Write("Enter Train Status: ");
                                            string trainStatus = Console.ReadLine();
                                            Console.Write("Enter Available Seats in A1: ");
                                            int availableSeatsInA1 = int.Parse(Console.ReadLine());
                                            Console.Write("Enter Available Seats in A2: ");
                                            int availableSeatsInA2 = int.Parse(Console.ReadLine());
                                            Console.Write("Enter Available Seats in A3: ");
                                            int availableSeatsInA3 = int.Parse(Console.ReadLine());
                                            Console.Write("Enter Total No. Of Seats in A1: ");
                                            int totalSeatsInA1 = int.Parse(Console.ReadLine());
                                            Console.Write("Enter Total No. Of Seats in A2: ");
                                            int totalSeatsInA2 = int.Parse(Console.ReadLine());
                                            Console.Write("Enter Total No. Of Seats in A3: ");
                                            int totalSeatsInA3 = int.Parse(Console.ReadLine());
                                            if (!_auth.VerifyAccessToken(_token))
                                            {
                                                _auth.RefereshAccessToken(_token);
                                            }
                                            bool result = IUDTrains(
                                                trainName: trainName, trainStatus: trainStatus,
                                                availableSeatsInA1: availableSeatsInA1, availableSeatsInA2: availableSeatsInA2,
                                                availableSeatsInA3: availableSeatsInA3, totalSeatsInA1: totalSeatsInA1,
                                                totalSeatsInA2: totalSeatsInA2, totalSeatsInA3: totalSeatsInA3, trainNo: out int trainNo
                                            );
                                            if (result)
                                            {
                                                Console.WriteLine($"New TrainNo: {trainNo}");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Train Registration Failed");
                                            }
                                            Thread.Sleep(1000);
                                            break;
                                        case 2:
                                            Console.Clear();
                                            Console.WriteLine("Displaying All Trains\n");
                                            DisplayTableData("Trains");
                                            Console.WriteLine();
                                            Console.Write("Enter Train No to UPDATE status: ");
                                            trainNo = int.Parse(Console.ReadLine().Trim());
                                            if(trainNo >= 10000 && trainNo <= 99999)
                                            {
                                                if(!_auth.VerifyAccessToken(_token))
                                                {
                                                    _auth.RefereshAccessToken(_token);
                                                }
                                                result = UpdateTrainStatus(trainNo, out string updatedTrainStatus);
                                                if(result)
                                                {
                                                    Console.WriteLine($"{trainNo} Train Status updated to {updatedTrainStatus}");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Train Status Updation Failed");
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Please Enter Valid Train No.");
                                            }
                                            Thread.Sleep(1000);
                                            break;
                                        case 3:
                                            Console.Clear();
                                            Console.WriteLine("Displaying All Trains\n");
                                            DisplayTableData("Trains");
                                            Console.WriteLine();
                                            Console.Write("Enter Train No to DELETE: ");
                                            trainNo = int.Parse(Console.ReadLine());
                                            if (trainNo >= 10000 && trainNo <= 99999)
                                            {
                                                if (!_auth.VerifyAccessToken(_token))
                                                {
                                                    _auth.RefereshAccessToken(_token);
                                                }
                                                result = IUDTrains(trainNo);
                                                if (result)
                                                {
                                                    Console.WriteLine($"Train {trainNo} DELETED Successfully");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Train Deletion Failed");
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Please Enter a Valid Train No");
                                            }
                                            Thread.Sleep(1000);
                                            break;
                                        case 4:
                                            Console.WriteLine("Exiting");
                                            Thread.Sleep(1000);
                                            break;
                                        default:
                                            Console.WriteLine("Invalid Choice");
                                            break;
                                    }
                                }
                                break;
                            case 3:
                                IUDChoice = 0;
                                while (IUDChoice != 4)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Press 1 to INSERT a Distance value");
                                    Console.WriteLine("Press 2 to UPDATE a Distance value");
                                    Console.WriteLine("Press 3 to DELETE a Distance value");
                                    Console.WriteLine("Press 4 to Exit");
                                    IUDChoice = int.Parse(Console.ReadLine());
                                    switch (IUDChoice)
                                    {
                                        case 1:
                                            Console.Clear();
                                            Console.WriteLine("Displaying Trains");
                                            DisplayTableData("Trains");
                                            Console.WriteLine();
                                            Console.Write("Enter Boarding Station:");
                                            string boardingStation = Console.ReadLine().Trim();
                                            Console.Write("Enter Destination Station: ");
                                            string destinationStation = Console.ReadLine().Trim();
                                            Console.Write("Enter Distance: ");
                                            float distance = float.Parse(Console.ReadLine().Trim());
                                            Console.Write("Enter Train No.: ");
                                            int trainNo = int.Parse(Console.ReadLine().Trim());
                                            if (!_auth.VerifyAccessToken(_token))
                                            {
                                                _auth.RefereshAccessToken(_token);
                                            }
                                            bool result = IUDDistance(boardingStation, destinationStation, distance, trainNo, out string distanceId);
                                            if (result)
                                            {
                                                Console.WriteLine($"Distance with ID {distanceId} created");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Distance Creation Failed");
                                            }
                                            Thread.Sleep(1000);
                                            break;
                                        case 2:
                                            Console.Clear();
                                            Console.WriteLine("Displaying Trains");
                                            DisplayTableData("Trains");
                                            Console.WriteLine();
                                            Console.WriteLine("Displaying Distances");
                                            DisplayTableData("Distance");
                                            Console.WriteLine();
                                            Console.Write("Enter Distance Id to UPDATE: ");
                                            distanceId = Console.ReadLine().Trim();
                                            Console.WriteLine("Enter <COLUMN NAME> : <COLUMN VALUE> to update");
                                            Dictionary<string, object> columnsToUpdate = new Dictionary<string, object>();
                                            string input = Console.ReadLine().Trim();
                                            while (input.Length > 0)
                                            {
                                                string[] keyValue = input.Split(':');
                                                if (keyValue.Length == 2)
                                                {
                                                    columnsToUpdate["@" + keyValue[0].Trim()] = keyValue[1].Trim();
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Please Enter values in format stated above");
                                                }
                                                input = Console.ReadLine().Trim();
                                            }
                                            if (!_auth.VerifyAccessToken(_token))
                                            {
                                                _auth.RefereshAccessToken(_token);
                                            }
                                            result = IUDDistance(distanceId, columnsToUpdate);
                                            if (result)
                                            {
                                                Console.WriteLine($"Distance with ID {distanceId} UPDATED Successfully");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Distance Updation Failed");
                                            }
                                            Thread.Sleep(1000);
                                            break;
                                        case 3:
                                            Console.Clear();
                                            Console.WriteLine("Displaying Distances");
                                            DisplayTableData("Distance");
                                            Console.WriteLine();
                                            Console.Write("Enter Distance ID to delete: ");
                                            distanceId = Console.ReadLine().Trim();
                                            if (!_auth.VerifyAccessToken(_token))
                                            {
                                                _auth.RefereshAccessToken(_token);
                                            }
                                            result = IUDDistance(distanceId);
                                            if (result)
                                            {
                                                Console.WriteLine($"Distance {distanceId} DELETED");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Distance Deletion Failed");
                                            }
                                            Thread.Sleep(1000);
                                            break;
                                        case 4:
                                            Console.WriteLine("Exiting");
                                            Thread.Sleep(1000);
                                            break;
                                        default:
                                            Console.WriteLine("Invalid Choice");
                                            break;
                                    }
                                }
                                break;
                            case 4:
                                IUDChoice = 0;
                                while (IUDChoice != 4)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Press 1 to INSERT a Berth Status");
                                    Console.WriteLine("Press 2 to UPDATE a Berth Status");
                                    Console.WriteLine("Press 3 to DELETE a Berth Status");
                                    Console.WriteLine("Press 4 to Exit");
                                    IUDChoice = int.Parse(Console.ReadLine().Trim());
                                    switch (IUDChoice)
                                    {
                                        case 1:
                                            Console.Clear();
                                            Console.Write("Enter Status Name: ");
                                            string statusName = Console.ReadLine().Trim();
                                            if (_auth.VerifyAccessToken(_token))
                                            {
                                                _auth.RefereshAccessToken(_token);
                                            }
                                            bool result = IUDBerthStatus(statusName, out string berthStatusId);
                                            if (result)
                                            {
                                                Console.WriteLine($"{berthStatusId} Berth Status Created Successfully");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Berth Status Creation Failed");
                                            }
                                            Thread.Sleep(1000);
                                            break;
                                        case 2:
                                            Console.Clear();
                                            Console.WriteLine("Displaying Berth Status Values");
                                            DisplayTableData("BerthStatus");
                                            Console.WriteLine();
                                            Console.Write("Enter BerthStatus ID to UPDATE: ");
                                            berthStatusId = Console.ReadLine().Trim();
                                            Console.Write("Enter Berth Status: ");
                                            statusName = Console.ReadLine().Trim();
                                            if (_auth.VerifyAccessToken(_token))
                                            {
                                                _auth.RefereshAccessToken(_token);
                                            }
                                            result = IUDBerthStatus(berthStatusId, statusName);
                                            if (result)
                                            {
                                                Console.WriteLine($"{berthStatusId} BerthStatus Updated Successfully");
                                            }
                                            else
                                            {
                                                Console.WriteLine("BerthStatus Updation Failed");
                                            }
                                            Thread.Sleep(1000);
                                            break;
                                        case 3:
                                            Console.Clear();
                                            Console.WriteLine("Displaying Berth Status Values");
                                            DisplayTableData("BerthStatus");
                                            Console.WriteLine();
                                            Console.Write("Enter BerthStatus ID to DELETE: ");
                                            berthStatusId = Console.ReadLine().Trim();
                                            if (_auth.VerifyAccessToken(_token))
                                            {
                                                _auth.RefereshAccessToken(_token);
                                            }
                                            result = IUDBerthStatus(berthStatusId);
                                            if (result)
                                            {
                                                Console.WriteLine($"{berthStatusId} Berth Status DELETED Successfully");
                                            }
                                            else
                                            {
                                                Console.WriteLine("BerthStatus Deletion Failed");
                                            }
                                            Thread.Sleep(1000);
                                            break;
                                        case 4:
                                            Console.WriteLine("Exiting");
                                            Thread.Sleep(1000);
                                            break;
                                        default:
                                            Console.WriteLine("Invalid Choice");
                                            break;
                                    }
                                }
                                break;
                            case 5:
                                IUDChoice = 0;
                                while (IUDChoice != 4)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Press 1 to INSERT a Berth Class");
                                    Console.WriteLine("Press 2 to UPDATE a Berth Class");
                                    Console.WriteLine("Press 3 to DELETE a Berth Class");
                                    Console.WriteLine("Press 4 to Exit.");
                                    IUDChoice = int.Parse(Console.ReadLine().Trim());
                                    switch (IUDChoice)
                                    {
                                        case 1:
                                            Console.Clear();
                                            Console.Write("Enter Class: ");
                                            string berthClass = Console.ReadLine().Trim();
                                            Console.Write("Enter PricePerKM: ");
                                            float pricePerKm = float.Parse(Console.ReadLine().Trim());
                                            if (!_auth.VerifyAccessToken(_token))
                                            {
                                                _auth.RefereshAccessToken(_token);
                                            }
                                            bool result = IUDBerthClass(berthClass, pricePerKm, out string berthClassId);
                                            if (result)
                                            {
                                                Console.WriteLine($"{berthClassId} Berth Class Created");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Berth Class INSERT Failed");
                                            }
                                            Thread.Sleep(1000);
                                            break;
                                        case 2:
                                            Console.Clear();
                                            Console.WriteLine("Displaying BerthClass Values");
                                            DisplayTableData("BerthClass");
                                            Console.WriteLine();
                                            Console.Write("Enter BerthClass ID to UPDATE: ");
                                            berthClassId = Console.ReadLine().Trim();
                                            Console.WriteLine("Enter <COLUMN NAME> : <COLUMN VALUE> to update");
                                            string input = Console.ReadLine().Trim();
                                            Dictionary<string, object> columnsToUpdate = new Dictionary<string, object>();
                                            while (input.Length > 0)
                                            {
                                                string[] keyValue = input.Split(':');
                                                if (keyValue.Length == 2)
                                                {
                                                    columnsToUpdate["@" + keyValue[0].Trim()] = keyValue[1].Trim();
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Please enter values in the format stated above");
                                                }
                                                input = Console.ReadLine().Trim();
                                            }
                                            if (!_auth.VerifyAccessToken(_token))
                                            {
                                                _auth.RefereshAccessToken(_token);
                                            }
                                            result = IUDBerthClass(berthClassId, columnsToUpdate);
                                            if (result)
                                            {
                                                Console.WriteLine($"{berthClassId} Berth Class UPDATED successfully");
                                            }
                                            else
                                            {
                                                Console.WriteLine("BerthClass updation failed");
                                            }
                                            Thread.Sleep(1000);
                                            break;
                                        case 3:
                                            Console.Clear();
                                            Console.WriteLine("Displaying BerthClass Values");
                                            DisplayTableData("BerthClass");
                                            Console.WriteLine();
                                            Console.Write("Enter BerthClass Id to DELETE: ");
                                            berthClassId = Console.ReadLine().Trim();
                                            if (!_auth.VerifyAccessToken(_token))
                                            {
                                                _auth.RefereshAccessToken(_token);
                                            }
                                            result = IUDBerthClass(berthClassId);
                                            if (result)
                                            {
                                                Console.WriteLine($"{berthClassId} BerthClass DELETED successfully");
                                            }
                                            else
                                            {
                                                Console.WriteLine("BerthClass Deletion Failed");
                                            }
                                            Thread.Sleep(1000);
                                            break;
                                        case 4:
                                            Console.WriteLine("Exiting.");
                                            Thread.Sleep(1000);
                                            break;
                                        default:
                                            Console.WriteLine("Invalid Choice");
                                            break;
                                    }
                                }
                                break;
                            case 6:
                                IUDChoice = 0;
                                while (IUDChoice != 4)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Press 1 to INSERT a Ticket Status");
                                    Console.WriteLine("Press 2 to UPDATE a Ticket Status");
                                    Console.WriteLine("Press 3 to DELETE a Ticket Status");
                                    Console.WriteLine("Press 4 to Exit.");
                                    IUDChoice = int.Parse(Console.ReadLine().Trim());
                                    switch (IUDChoice)
                                    {
                                        case 1:
                                            Console.Clear();
                                            Console.Write("Enter Ticket Status: ");
                                            string ticketStatus = Console.ReadLine().Trim();
                                            if (!_auth.VerifyAccessToken(_token))
                                            {
                                                _auth.RefereshAccessToken(_token);
                                            }
                                            bool result = IUDTicketStatus(ticketStatus, out string ticketStatusId);
                                            if (result)
                                            {
                                                Console.WriteLine($"{ticketStatusId} Ticket Status Created Successfully");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Ticket Status Creation Failed");
                                            }
                                            Thread.Sleep(1000);
                                            break;
                                        case 2:
                                            Console.Clear();
                                            Console.WriteLine("Displaying Ticket Status Values");
                                            DisplayTableData("TicketStatus");
                                            Console.WriteLine();
                                            Console.Write("Enter TicketStatus ID to UPDATE: ");
                                            ticketStatusId = Console.ReadLine().Trim();
                                            Console.WriteLine("Enter <COLUMN NAME> : <COLUMN VALUE> to UPDATE");
                                            string input = Console.ReadLine().Trim();
                                            Dictionary<string, object> columnsToUpdate = new Dictionary<string, object>();
                                            while (input.Length > 0)
                                            {
                                                string[] keyValue = input.Split(':');
                                                if (keyValue.Length == 2)
                                                {
                                                    columnsToUpdate["@" + keyValue[0].Trim()] = keyValue[1].Trim();
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Please Enter in the format stated above");
                                                }
                                                input = Console.ReadLine().Trim();
                                            }
                                            if (!_auth.VerifyAccessToken(_token))
                                            {
                                                _auth.RefereshAccessToken(_token);
                                            }
                                            result = IUDTicketStatus(ticketStatusId, columnsToUpdate);
                                            if (result)
                                            {
                                                Console.WriteLine($"{ticketStatusId} Ticket Status Updated Successfully");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Ticket Status Updation Failed");
                                            }
                                            Thread.Sleep(1000);
                                            break;
                                        case 3:
                                            Console.Clear();
                                            Console.WriteLine("Displaying TicketStatus Values");
                                            DisplayTableData("TicketStatus");
                                            Console.WriteLine();
                                            Console.Write("Enter TicketStatus ID to DELETE: ");
                                            ticketStatusId = Console.ReadLine().Trim();
                                            if (!_auth.VerifyAccessToken(_token))
                                            {
                                                _auth.RefereshAccessToken(_token);
                                            }
                                            result = IUDTicketStatus(ticketStatusId);
                                            if (result)
                                            {
                                                Console.WriteLine($"{ticketStatusId} Ticket Status DELETED Successfully");
                                            }
                                            else
                                            {
                                                Console.WriteLine("TicketStatus Deletion Failed");
                                            }
                                            Thread.Sleep(1000);
                                            break;
                                        case 4:
                                            Console.WriteLine("Exiting.");
                                            Thread.Sleep(1000);
                                            break;
                                        default:
                                            Console.WriteLine("Invalid Choice");
                                            break;
                                    }
                                }
                                break;
                            case 7:
                                IUDChoice = 0;
                                while (IUDChoice != 4)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Press 1 to INSERT a Role");
                                    Console.WriteLine("Press 2 to UPDATE a Role");
                                    Console.WriteLine("Press 3 to DELETE a Role");
                                    Console.WriteLine("Press 4 to Exit.");
                                    IUDChoice = int.Parse(Console.ReadLine().Trim());
                                    switch (IUDChoice)
                                    {
                                        case 1:
                                            Console.Clear();
                                            Console.Write("Enter Role: ");
                                            string role = Console.ReadLine().Trim();
                                            if (!_auth.VerifyAccessToken(_token))
                                            {
                                                _auth.RefereshAccessToken(_token);
                                            }
                                            bool result = IUDRoles(role, out string roleId);
                                            if (result)
                                            {
                                                Console.WriteLine($"{roleId} Role Created Successfully");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Role Creation Failed");
                                            }
                                            Thread.Sleep(1000);
                                            break;
                                        case 2:
                                            Console.Clear();
                                            Console.WriteLine("Displaying Role Values");
                                            DisplayTableData("Roles");
                                            Console.WriteLine();
                                            Console.Write("Enter Role ID to UPDATE: ");
                                            roleId = Console.ReadLine().Trim();
                                            Console.WriteLine("Enter <COLUMN NAME> : <COLUMN VALUE> to UPDATE");
                                            string input = Console.ReadLine().Trim();
                                            Dictionary<string, object> columnsToUpdate = new Dictionary<string, object>();
                                            while (input.Length > 0)
                                            {
                                                string[] keyValue = input.Split(':');
                                                if (keyValue.Length == 2)
                                                {
                                                    columnsToUpdate["@" + keyValue[0].Trim()] = keyValue[1].Trim();
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Please enter values in format stated above");
                                                }
                                                input = Console.ReadLine().Trim();
                                            }
                                            if (!_auth.VerifyAccessToken(_token))
                                            {
                                                _auth.RefereshAccessToken(_token);
                                            }
                                            result = IUDRoles(roleId, columnsToUpdate);
                                            if (result)
                                            {
                                                Console.WriteLine($"{roleId} UPDATED Successfully");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Role Updation Failed");
                                            }
                                            Thread.Sleep(1000);
                                            break;
                                        case 3:
                                            Console.Clear();
                                            Console.WriteLine("Displaying Role Values");
                                            DisplayTableData("Roles");
                                            Console.WriteLine();
                                            Console.Write("Enter Role ID to DELETE: ");
                                            roleId = Console.ReadLine().Trim();
                                            if (!_auth.VerifyAccessToken(_token))
                                            {
                                                _auth.RefereshAccessToken(_token);
                                            }
                                            result = IUDRoles(roleId);
                                            if (result)
                                            {
                                                Console.WriteLine($"{roleId} DELETED Successfully");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Role Deletion Failed");
                                            }
                                            Thread.Sleep(1000);
                                            break;
                                        case 4:
                                            Console.WriteLine("Exiting");
                                            Thread.Sleep(1000);
                                            break;
                                        default:
                                            Console.WriteLine("Invalid Choice");
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine(ex.Message);
                        Thread.Sleep(1000);
                    }
                    
                }
            }
            return;
        }
    }
}
