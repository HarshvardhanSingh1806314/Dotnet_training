using System;

namespace Assignment_3
{
    internal class StringAssignments
    {
        // This function displays the length of the inputted string
        static void DisplayStringLength(string str)
        {
            Console.WriteLine($"Length of {str}: {str.Length}");
        }

        // This function displays the reverse of the inputted string
        static void DisplayReverseString(string str)
        {
            char[] stringToChars = str.ToCharArray();
            Array.Reverse(stringToChars);
            Console.WriteLine($"Reverse of {str}: {new string(stringToChars)}");
        }

        // This function checks whether inputted strings are equal in value or not
        static bool AreStringsEqual(string str1, string str2)
        {
            return str1.Equals(str2);
        }

        static void Main(string[] args)
        {
            // taking string input for DisplayStringLength function
            {
                Console.WriteLine("Taking string input for DisplayStringLength function");
                Console.Write("Enter string: ");
                string str = Console.ReadLine();
                DisplayStringLength(str);
            }

            Console.WriteLine("------------------------------------------------------------");
            // taking string input for DisplayReverseString function
            {
                Console.WriteLine("Taking string input for DisplayReverseString function");
                Console.Write("Enter string: ");
                string str = Console.ReadLine();
                DisplayReverseString(str);
            }

            Console.WriteLine("------------------------------------------------------------");
            // taking string inputs for AreStringEqual function
            {
                Console.WriteLine("Taking string inputs for AreStringEqual function");
                Console.Write("Enter string 1: ");
                string str1 = Console.ReadLine();

                Console.Write("Enter string 2: ");
                string str2 = Console.ReadLine();

                string result = AreStringsEqual(str1, str2) ? "Equal" : "Not Equal";
                Console.WriteLine($"{str1} and {str2} are {result}");
            }

            Console.ReadLine();
        }
    }
}
