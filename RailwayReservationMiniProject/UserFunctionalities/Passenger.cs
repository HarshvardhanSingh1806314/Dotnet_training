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
    public class Passenger : User
    {
        public Passenger(SqlConnection connection, string role) : base(connection, role) { }

        private bool BookTicket(int trainNo, string berthClass, string boardingStation, string destinationStation, out int generatedPnrNo,
            float pricePerKmForA1 = 0.0f, float pricePerKmForA2 = 0.0f, float pricePerKmForA3 = 0.0f
        )
        {
            try
            {
                _connection.Open();

                // checking if the seat is available in selected berth class if yes then generating a berth no to book from the available berths
                string query = $"select availableSeatsIn{berthClass} from Trains where trainNo = @trainNo";
                SqlCommand sqlCommand = new SqlCommand(query, _connection);
                sqlCommand.Parameters.AddWithValue("@trainNo", trainNo);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                int berthNo = 0;
                while (reader.Read())
                {
                    int noOfSeatsAvailable = int.Parse(reader[0].ToString());
                    if(noOfSeatsAvailable > 0)
                    {
                        Random berthNoGenerator = new Random();
                        berthNo = berthNoGenerator.Next(1, noOfSeatsAvailable);
                    }
                    else
                    {
                        throw new InsufficientSeatsException($"Not Enough Seats Available in Berth Class {berthClass}");
                    }
                }
                sqlCommand.Parameters.Clear();
                reader.Close();

                // generating a new pnr no for ticket
                query = "select pnrNo from Tickets";
                sqlCommand.CommandText = query;
                reader = sqlCommand.ExecuteReader();
                ArrayList listOfExistingPnrNos = new ArrayList();
                while(reader.Read())
                {
                    listOfExistingPnrNos.Add(reader.GetInt32(0));
                }
                generatedPnrNo = GenerateIds.GeneratePnrNo(listOfExistingPnrNos);
                reader.Close();

                // calculating total price for the journey
                query = "select distance from Distance where boardingStation = @boardingStation and destination = @destinationStation and train = @trainNo";
                sqlCommand.CommandText = query;
                sqlCommand.Parameters.AddWithValue("@boardingStation", boardingStation);
                sqlCommand.Parameters.AddWithValue("@destinationStation", destinationStation);
                sqlCommand.Parameters.AddWithValue("@trainNo", trainNo);
                reader = sqlCommand.ExecuteReader();
                float totalPriceForJourney = 0.0f;
                while(reader.Read())
                {
                    totalPriceForJourney = float.Parse(reader[0].ToString()) * (pricePerKmForA1 > 0.0f ? pricePerKmForA1 : pricePerKmForA2 > 0.0f ? pricePerKmForA2 : pricePerKmForA3);
                }
                sqlCommand.Parameters.Clear();
                reader.Close();

                // fetching the id for Confirmed ticket status
                query = "select id from TicketStatus where statusName = @statusName";
                sqlCommand.CommandText = query;
                sqlCommand.Parameters.AddWithValue("@statusName", (object)"CONFIRMED");
                reader = sqlCommand.ExecuteReader();
                string ticketStatus = null;
                while(reader.Read())
                {
                    ticketStatus = reader[0].ToString();
                }
                sqlCommand.Parameters.Clear();
                reader.Close();

                // creating ticket to insert into database
                query = "insert into Tickets values(@pnrNo, @passengerId, @trainNo, @berthNo, @berthClass, @price, @ticketStatus, @bookingDate)";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    {"@pnrNo", generatedPnrNo },
                    {"@passengerId", _token.UserId },
                    {"@trainNo", trainNo },
                    {"@berthNo", berthNo },
                    {"@berthClass", berthClass },
                    {"@price", totalPriceForJourney },
                    {"@ticketStatus", ticketStatus},
                    {"@bookingDate", DateTime.Now }
                };
                _connection.Close();
                return ExecuteQuery(query, parameters);
            }
            catch (InsufficientSeatsException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

            generatedPnrNo = 0;
            return false;
        }

        private bool GetTrains(out int pnrNo)
        {
            try
            {
                _connection.Open();
                Console.Write("Enter Boarding Station: ");
                string boardingStation = Console.ReadLine().Trim();
                Console.Write("Enter Destination Station: ");
                string destinationStation = Console.ReadLine().Trim();

                // fetching the price per km for all the berth classes
                float pricePerKmForA1 = 0.0F, pricePerKmForA2 = 0.0F, pricePerKmForA3 = 0.0F;
                string query = "select pricePerkm from BerthClass where class = @class";
                SqlCommand sqlCommand = new SqlCommand(query, _connection);
                SqlDataReader sqlDataReader = null;
                for (int i=1;i<=3;i++)
                {
                    sqlCommand.Parameters.AddWithValue("@class", (object)$"A{i}");
                    sqlDataReader = sqlCommand.ExecuteReader();
                    while(sqlDataReader.Read())
                    {
                        switch(i)
                        {
                            case 1:
                                pricePerKmForA1 = float.Parse(sqlDataReader[0].ToString());
                                break;
                            case 2:
                                pricePerKmForA2 = float.Parse(sqlDataReader[0].ToString());
                                break;
                            case 3:
                                pricePerKmForA3 = float.Parse(sqlDataReader[0].ToString());
                                break;
                        }
                    }
                    sqlCommand.Parameters.Clear();
                    sqlDataReader.Close();
                }

                // fetching all the trains that are available between give boarding station and destination station
                query = "select train, distance from Distance where boardingStation = @boardingStation and destination = @destinationStation";
                sqlCommand.CommandText = query;
                sqlCommand.Parameters.AddWithValue("@boardingStation", boardingStation);
                sqlCommand.Parameters.AddWithValue("destinationStation", destinationStation);
                sqlDataReader = sqlCommand.ExecuteReader();
                List<Tuple<int, float>> trainAndDistance = new List<Tuple<int, float>>();
                while(sqlDataReader.Read())
                {
                    trainAndDistance.Add(new Tuple<int, float>(int.Parse(sqlDataReader[0].ToString()), float.Parse(sqlDataReader [1].ToString())));
                }
                sqlCommand.Parameters.Clear();
                sqlDataReader.Close();

                // Displaying All the available trains
                Console.WriteLine($"\nAll the Available Trains For {boardingStation} To {destinationStation}\n");
                foreach(Tuple<int, float> trainDistancePair in trainAndDistance)
                {
                    query = "select trainNo, trainName, availableSeatsInA1, availableSeatsInA2, availableSeatsInA3 from Trains" +
                        " where trainNo = @trainNo and trainStatus = (select id from TrainStatus where statusName = @statusName)";
                    sqlCommand.CommandText = query;
                    sqlCommand.Parameters.AddWithValue("@statusName", (object)"RUNNING");
                    sqlCommand.Parameters.AddWithValue("@trainNo", trainDistancePair.Item1);
                    sqlDataReader = sqlCommand.ExecuteReader();
                    while(sqlDataReader.Read())
                    {
                        Console.WriteLine($"Train No.: {sqlDataReader[0]}");
                        Console.WriteLine($"Train Name: {sqlDataReader[1]}");
                        Console.WriteLine($"Available Seats In A1: {sqlDataReader[2]}");
                        Console.WriteLine($"Available Seats In A2: {sqlDataReader[3]}");
                        Console.WriteLine($"Available Seats In A3: {sqlDataReader[4]}");
                        Console.WriteLine($"Total Price For A1: {pricePerKmForA1 * trainDistancePair.Item2}");
                        Console.WriteLine($"Total Price For A2: {pricePerKmForA2 * trainDistancePair.Item2}");
                        Console.WriteLine($"Total Price For A3: {pricePerKmForA3 * trainDistancePair.Item2}");
                        Console.WriteLine("\n-------------------------------------------------------------------------\n");
                    }
                    sqlCommand.Parameters.Clear();
                    sqlDataReader.Close();
                }
                _connection.Close();

                Console.Write("Enter Train No. for which you want to book ticket: ");
                int trainNo = int.Parse(Console.ReadLine().Trim());
                Console.Write("Enter the Berth Class: ");
                string berthClass = Console.ReadLine().Trim().ToUpper();
                if(berthClass == "A1")
                {
                    return BookTicket(
                        trainNo,
                        berthClass, 
                        boardingStation, 
                        destinationStation, 
                        out pnrNo,
                        pricePerKmForA1 : pricePerKmForA1);
                }
                else if(berthClass == "A2")
                {
                    return BookTicket(
                        trainNo, 
                        berthClass, 
                        boardingStation, 
                        destinationStation, 
                        out pnrNo,
                        pricePerKmForA2 : pricePerKmForA2
                    );
                }
                else if(berthClass == "A3")
                {
                    return BookTicket(
                        trainNo, 
                        berthClass, 
                        boardingStation, 
                        destinationStation, 
                        out pnrNo, 
                        pricePerKmForA3 : pricePerKmForA3
                    );
                }
            }
            catch(FormatException ex)
            {
                Console.WriteLine(ex.Message);
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Thread.Sleep(1000);
            }
            finally
            {
                _connection.Close();
            }

            pnrNo = 0;
            return false;
        }

        private bool CancelTicket(int pnrNo)
        {
            try
            {
                _connection.Open();

                // fetching the id for CANCELLED TicketStatus
                string query = "select id from TicketStatus where statusName = @statusName";
                SqlCommand sqlCommand = new SqlCommand(query, _connection);
                sqlCommand.Parameters.AddWithValue("@statusName", (object)"CANCELLED");
                SqlDataReader reader = sqlCommand.ExecuteReader();
                string ticketStatusId = null;
                while(reader.Read())
                {
                    ticketStatusId = reader[0].ToString();
                }
                sqlCommand.Parameters.Clear();
                reader.Close();
                _connection.Close();

                // updating ticket status to CANCELLED
                query = "update Tickets set ticketStatus = @ticketStatus where pnrNo = @pnrNo";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    {"@ticketStatus", ticketStatusId },
                    {"@pnrNo", pnrNo},
                };
                return ExecuteQuery(query, parameters);
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

        private void AuthenticatePassenger()
        {
            int choice = 0;
            while (choice != 3)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("Welcome To PASSENGER area");
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
                                choice = 3;
                            }
                            break;
                        case 2:
                            bool isUserRegistered = Register();
                            if (isUserRegistered)
                            {
                                Console.WriteLine("You are being redirected to login page");
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
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                    Thread.Sleep(1000);
                }
            }
        }

        private void DisplayAllConfirmedTickets()
        {
            try
            {
                _connection.Open();

                // fetching id of CONFIRMED TicketStatus
                string query = "select id from TicketStatus where statusName = @statusName";
                SqlCommand sqlCommand = new SqlCommand(query, _connection);
                sqlCommand.Parameters.AddWithValue("@statusName", (object)"CONFIRMED");
                SqlDataReader reader = sqlCommand.ExecuteReader();
                string confirmedTicketStatusId = null;
                while(reader.Read())
                {
                    confirmedTicketStatusId = reader[0].ToString();
                }
                sqlCommand.Parameters.Clear();
                reader.Close();

                // fetching all the confirmed tickets of the user
                query = "select pnrNo, trainNo, berthNo, berthClass, price, bookingDate from Tickets where passengerId = @userId and ticketStatus = @confirmedTicketStatusId";
                sqlCommand.CommandText = query;
                sqlCommand.Parameters.AddWithValue("@userId", _token.UserId);
                sqlCommand.Parameters.AddWithValue("@confirmedTicketStatusId", confirmedTicketStatusId);
                reader = sqlCommand.ExecuteReader();
                while(reader.Read())
                {
                    Console.WriteLine($"PnrNo: {reader[0]}");
                    Console.WriteLine($"TrainNo: {reader[1]}");
                    Console.WriteLine($"BerthNo: {reader[2]}");
                    Console.WriteLine($"BerthClass: {reader[3]}");
                    Console.WriteLine($"Price: {reader[4]}");
                    Console.WriteLine($"TicketStatus: CONFIRMED");
                    Console.WriteLine($"Booking Date: {reader[5]}");
                    Console.WriteLine("\n-----------------------------------------------------------\n");
                }
                sqlCommand.Parameters.Clear();
                reader.Close();
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

        private void DisplayAllTickets()
        {
            try
            {
                // fetching the ticketStatus for all the tickets of user
                string query = "select * from TicketStatus";
                DataTable dataTable = new DataTable();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, _connection);
                sqlDataAdapter.Fill(dataTable);
                Dictionary<object, object> ticketStatusValues = new Dictionary<object, object>();
                foreach(DataRow row in dataTable.Rows)
                {
                    ticketStatusValues.Add(row[0], row[1]);
                }
                dataTable.Clear();

                // fetching the details of ticket
                query = "select pnrNo, trainNo, berthNo, berthClass, price, ticketStatus, bookingDate from Tickets where passengerId = @userId";
                sqlDataAdapter.SelectCommand.CommandText = query;
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@userId", _token.UserId);
                sqlDataAdapter.Fill(dataTable);
                foreach(DataRow row in dataTable.Rows)
                {
                    Console.WriteLine($"Pnr No.: {row["pnrNo"]}");
                    Console.WriteLine($"Train No.: {row["trainNo"]}");
                    Console.WriteLine($"Berth No.: {row["berthNo"]}");
                    Console.WriteLine($"Berth Class: {row["berthClass"]}");
                    Console.WriteLine($"Price: {row["price"]}");
                    Console.WriteLine($"Ticket Status: {ticketStatusValues[row["ticketStatus"]]}");
                    Console.WriteLine($"Booking Date: {row["bookingDate"]}");
                    Console.WriteLine("\n--------------------------------------------------------------------------\n");
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

        private void DisplayUserInterface()
        {
            Console.Clear();
            Console.WriteLine("Press 1 to BOOK TICKETS");
            Console.WriteLine("Press 2 to CANCEL TICKETS");
            Console.WriteLine("Press 3 to SHOW ALL BOOKINGS");
            Console.WriteLine("Press 4 to EXIT");
        }

        public void EnterPassengerArea()
        {
            // Authenticating Passenger
            AuthenticatePassenger();

            // checking if the passenger has access token or not
            if(_token != null)
            {
                int choice = 0;
                while(choice != 4)
                {
                    DisplayUserInterface();
                    try
                    {
                        Console.Write("Enter Your Choice: ");
                        choice = int.Parse(Console.ReadLine());
                        switch (choice)
                        {
                            case 1:
                                Console.Clear();
                                if (_auth.VerifyAccessToken(_token))
                                {
                                    _auth.RefereshAccessToken(_token);
                                }
                                if (GetTrains(out int pnrNo))
                                {
                                    Console.WriteLine($"Ticket with PnrNo {pnrNo} Booked");
                                }
                                else
                                {
                                    Console.WriteLine("Ticket Booking Failed.\nTry Again Later!");
                                }
                                Thread.Sleep(2000);
                                break;
                            case 2:
                                Console.Clear();
                                Console.WriteLine("Displaying All Your Confirmed Tickets\n");
                                DisplayAllConfirmedTickets();
                                Console.Write("Enter PnrNo. of Ticket to Cancel: ");
                                pnrNo = int.Parse(Console.ReadLine().Trim());
                                if(pnrNo < 1000000000)
                                {
                                    throw new FormatException("Invalid PNR Number");
                                }
                                if (!_auth.VerifyAccessToken(_token))
                                {
                                    _auth.RefereshAccessToken(_token);
                                }
                                bool result = CancelTicket(pnrNo);
                                if (result)
                                {
                                    Console.WriteLine($"Ticket {pnrNo} successfully Cancelled");
                                }
                                else
                                {
                                    Console.WriteLine("Ticket Cancellation Failed");
                                }
                                Thread.Sleep(1000);
                                break;
                            case 3:
                                Console.Clear();
                                Console.WriteLine("Displaying All Ticket Bookings\n");
                                DisplayAllTickets();
                                Console.WriteLine("Press Any Key to Exit");
                                Console.ReadLine();
                                break;
                            case 4:
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
                }
            }
            return;
        }
    }
}
