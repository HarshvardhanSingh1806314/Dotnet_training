using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_6
{
    internal class SquaresQuery
    {
        static void Main()
        {
            Console.Write("Enter size of array: ");
            int n = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter elements in the array");
            int[] arr = new int[n];
            for(int i=0;i<n;i++)
            {
                arr[i] = int.Parse(Console.ReadLine());
            }

            // writing query to return squares if square is greater than 20
            IEnumerable<int> list = arr.Where<int>(x => x * x > 20);
            Console.WriteLine("\nPrinting numbers with their squares");
            foreach(int element in list)
            {
                Console.WriteLine($"Number: {element}, Square: {element * element}");
            }

            Console.ReadLine();
        }
    }
}
