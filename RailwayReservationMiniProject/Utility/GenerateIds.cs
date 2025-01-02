using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace Utility
{
    public class GenerateIds
    {
        private static string GenerateCustomId(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] inputBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(inputBytes);
            }
        }

        public static string GenerateUserId(string userRole, string phoneNumber)
        {
            string input = $"{userRole.ToUpper()}{DateTime.Now}{phoneNumber}";
            return GenerateCustomId(input);
        }

        public static string GenerateRolesId(string roleName) 
        {
            string input = $"{roleName.ToUpper()}{DateTime.Now}";
            return GenerateCustomId(input);
        }

        public static string GenerateBerthClassId(string berthType, float pricePerKm)
        {
            string input = $"{berthType}{pricePerKm}{DateTime.Now}";
            return GenerateCustomId(input);
        }

        public static string GenerateBerthStatusId(string berthStatus)
        {
            string input = $"{berthStatus}{DateTime.Now}";
            return GenerateCustomId(input);
        }

        public static string GenerateDistanceId(string boardingStation, string destination, float distance)
        {
            string input = $"{boardingStation}{destination}{distance}{DateTime.Now}";
            return GenerateCustomId(input);
        }

        public static string GenerateTicketStatusId(string ticketStatus)
        {
            string input = $"{ticketStatus}{DateTime.Now}";
            return GenerateCustomId(input);
        }

        public static string GenerateTrainStatusId(string trainStatus)
        {
            string input = $"{trainStatus}{DateTime.Now}";
            return GenerateCustomId(input);
        }

        public static string GeneratePasswordId(string email, string password)
        {
            string input = $"{email}{password}{DateTime.Now}";
            return GenerateCustomId(input);
        }

        public static int GenerateTrainNo(ArrayList listOfExistingTrainNumbers)
        {
            Random trainNoGenerator = new Random();
            int trainNo = trainNoGenerator.Next(10000, 99999);
            while(listOfExistingTrainNumbers.BinarySearch(trainNo) >= 0)
            {
                trainNo = trainNoGenerator.Next(10000, 99999);
            }
            return trainNo;
        }

        public static int GeneratePnrNo(ArrayList listOfExistingPnrNos)
        {
            Random pnrNoGenerator = new Random();
            int pnrNo = pnrNoGenerator.Next(1000000000, int.MaxValue);
            while(listOfExistingPnrNos.BinarySearch(pnrNo) >= 0)
            {
                pnrNo = pnrNoGenerator.Next(1000000000, int.MaxValue);
            }
            return pnrNo;
        }
    }
}
