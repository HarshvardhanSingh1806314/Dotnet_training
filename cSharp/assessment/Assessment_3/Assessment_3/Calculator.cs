using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment_3
{
    internal class Calculator
    {
        private delegate int Operate(int x, int y);

        static int PerformOperation(int x, int y, Operate operate)
        {
            return operate(x, y);
        }
        public static void Main()
        {
            Console.Write("Enter first number: ");
            int x = int.Parse(Console.ReadLine());
            Console.Write("Enter operator: ");
            string oprator = Console.ReadLine();
            Console.Write("Enter second number: ");
            int y = int.Parse(Console.ReadLine());

            int result = int.MinValue;
            switch(oprator)
            {
                case "+":
                    Operate add = new Operate(Add);
                    result = PerformOperation(x, y, add);
                    break;
                case "-":
                    Operate subtract = new Operate(Subtract);
                    result = PerformOperation(x, y, subtract);
                    break;
                case "*":
                    Operate multiply = new Operate(Multiply);
                    result = PerformOperation(x, y, multiply);
                    break;
                default:
                    Console.WriteLine("Invalid operator");
                    break;
            }

            if(result != int.MinValue)
                Console.WriteLine($"{x} {oprator} {y} = {result}");

            Console.ReadLine();
        }

        static int Add(int x, int y)
        {
            return x + y;
        }

        static int Subtract(int x, int y)
        {
            return x - y;
        }

        static int Multiply(int x, int y)
        {
            return x * y;
        }
    }
}
