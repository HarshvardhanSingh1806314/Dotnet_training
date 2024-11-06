using System;

namespace Assessment_1
{
    internal class Program
    {
        // function to remove a character from a specified index in a string
        static string RemoveCharacter(string str, int index)
        {
            return str.Remove(index, 1);
        }

        // function to exchange first and last character in a string
        static string ExchangeFirstAndLastCharacter(string str)
        {
            string exchangedString = str[str.Length - 1] + str.Substring(1, str.Length - 2)+ str[0];
            return exchangedString;
        }

        // function to check which is the largest among three numbers
        static void CheckLargest(int num1, int num2, int num3)
        {
            int largest = Math.Max(num1, Math.Max(num2, num3));
            Console.WriteLine($"Largest among {num1}, {num2}, {num3}: {largest}");
        }

        // function to count occurrences of a character in a string
        static void CountOccurences(string str, char ch)
        {
            int count = 0;
            foreach(char c in str)
            {
                if (c.Equals(ch))
                    count++;
            }
            Console.WriteLine($"Occurrences of \"{ch}\" : {count}");
        }
        static void Main()
        {
            // taking inputs for RemoveCharacter function
            {
                Console.WriteLine("Taking inputs for RemoveCharacter function");
                Console.Write("Enter String: ");
                string str = Console.ReadLine();
                Console.Write("Enter the Position: ");
                int position = int.Parse(Console.ReadLine());
                Console.WriteLine($"String before character removed: {str}");
                str = RemoveCharacter(str, position);
                Console.WriteLine($"String after Character Removed: {str}");
            }

            Console.WriteLine("-----------------------------------------------------------");

            // taking inputs for ExchangeFirstAndLastCharacter function
            {
                Console.WriteLine("Taking inputs for ExhangeFirstAndLastCharacter function");
                Console.Write("Enter string: ");
                string str = Console.ReadLine();
                Console.WriteLine($"String before exchange: {str}");
                str = ExchangeFirstAndLastCharacter(str);
                Console.WriteLine($"String after exchange {str}");
            }

            Console.WriteLine("-----------------------------------------------------------");

            // taking inputs for CheckLargest function
            {
                Console.WriteLine("Taking inputs for CheckLargest function");
                Console.Write("Enter 1st Number: ");
                int num1 = int.Parse(Console.ReadLine());
                Console.Write("Enter 2nd Number: ");
                int num2 = int.Parse(Console.ReadLine());
                Console.Write("Enter 3rd Number: ");
                int num3 = int.Parse(Console.ReadLine());
                CheckLargest(num1, num2, num3);
            }

            Console.WriteLine("-----------------------------------------------------------");

            // taking inputs for CountOccurrences function
            {
                Console.WriteLine("Taking inputs for CountOccurrences function");
                Console.Write("Enter string: ");
                string str = Console.ReadLine();
                Console.Write("Enter Character to count occurrences for: ");
                string ch = Console.ReadLine();
                if (str.Contains(ch))
                {
                    CountOccurences(str, ch.ToCharArray()[0]);
                }
                else
                    Console.WriteLine($"\"{ch}\" does not exist in {str}");
            }
            Console.ReadLine();
        }
    }
}
