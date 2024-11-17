using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_6
{
    internal class WordQuery
    {
        public static void Main()
        {
            Console.Write("Enter size of words array: ");
            int n = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter elements in array");
            string[] arr = new string[n];
            for(int i=0;i<n;i++)
            {
                arr[i] = Console.ReadLine();
            }

            // writing query to find words starting with 'a' and ending with 'm'
            IEnumerable<string> list = arr.Where<string>(str => str.StartsWith("a") && str.EndsWith("m"));
            Console.WriteLine("\nPrinting words");
            foreach(String word in list)
            {
                Console.WriteLine(word);
            }

            Console.ReadLine();
        }
    }
}
