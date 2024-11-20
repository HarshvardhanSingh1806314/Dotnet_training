using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment_3.BoxOperations
{
    internal class Test
    {
        public static void Main()
        {
            // taking box 1 details
            Console.WriteLine("Enter Details of box 1");
            Console.Write("Length: ");
            float length = float.Parse(Console.ReadLine());
            Console.Write("Width: ");
            float width = float.Parse(Console.ReadLine());
            Box box1 = new Box(length, width);

            // taking box 2 details
            Console.WriteLine("Enter Details of box 2");
            Console.Write("Length: ");
            length = float.Parse(Console.ReadLine());
            Console.Write("Width: ");
            width = float.Parse(Console.ReadLine());
            Box box2 = new Box(length, width);

            // Adding both the boxes
            Box result = box1 + box2;
            Console.WriteLine("\nResulting Box Dimensions after adding box1 and box2");
            result.Display();

            Console.ReadLine();
        }
    }
}
