using System;

namespace Assignment1
{
    internal class Assignments
    {
        // function to check whether two integers are equal or not
        static bool EqualOrNot(int num1, int num2)
        {
            return num1 == num2;
        }

        // function to check whether the integer is positive or negative
        static void PositiveOrNegative(dynamic num)
        {
            if (num > 0)
                Console.WriteLine($"{num} is Positive");
            else
                Console.WriteLine($"{num} is Negative");
        }
        
        // Basic Calculator
        static void Operate(dynamic num1, dynamic num2, char oprator)
        {
            switch(oprator)
            {
                case '+':
                    Console.WriteLine(num1 + num2);
                    break;
                case '-':
                    Console.WriteLine(num1 - num2);
                    break;
                case '/':
                    if (num2 != 0)
                        Console.WriteLine(num1 / num2);
                    else
                        Console.WriteLine("Cannot divide by 0");
                    break;
                case '*':
                    Console.WriteLine(num1 * num2);
                    break;
                default:
                    Console.WriteLine("Invalid Operator");
                    break;
            }
        }

        // function to print multiplication table
        static void PrintMultiplicationTable(int num)
        {
            for(int i=0;i<=10;i++)
            {
                Console.WriteLine($"{num} * {i} = {num * i}");
            }
        }

        // function to calculate sum of two numbers
        static int CalculateSum(int num1, int num2)
        {
            return num1 == num2 ? (num1 + num2) * 3 : num1 + num2;
        }
        static void Main(string[] args)
        {
            // input values for function to check whether both values are equal or not
            {
                Console.WriteLine("Input values for function to check whether both values are equal or not");
                Console.Write("Input 1st number: ");
                int num1 = int.Parse(Console.ReadLine());

                Console.Write("Input 2nd number: ");
                int num2 = int.Parse(Console.ReadLine());

                string result = EqualOrNot(num1, num2) ? "Equal" : "Not Equal";
                Console.WriteLine($"Output: {num1} and {num2} are {result}");
            }

            Console.WriteLine("\n---------------------------------------------\n");

            // input values for function to check whether value is positive or negative
            {
                Console.WriteLine("Input values for function to check whether value is positive or negative");
                Console.Write("Value: ");
                int num = int.Parse(Console.ReadLine());
                PositiveOrNegative(num);
            }

            Console.WriteLine("\n---------------------------------------------\n");

            // input values for basic calculator function
            {
                Console.WriteLine("Input values for basic calculator function");
                Console.Write("Input 1st Number: ");
                double num1 = double.Parse(Console.ReadLine());

                Console.Write("Input Operation: ");
                char oprator = char.Parse(Console.ReadLine());

                Console.Write("Input 2nd Number: ");
                double num2 = double.Parse(Console.ReadLine());

                Operate(num1, num2, oprator);
            }

            Console.WriteLine("\n---------------------------------------------\n");

            // input values for function printing multiplication table
            {
                Console.Write("Enter the number whose multiplication table you want to print: ");
                int num = int.Parse(Console.ReadLine());
                PrintMultiplicationTable(num);
            }

            Console.WriteLine("\n---------------------------------------------\n");

            // input values for function calculating sum
            {
                Console.WriteLine("Input values for function calculating sum of two values and triple the sum if both values are equal");
                Console.Write("Input 1st Number: ");
                int num1 = int.Parse(Console.ReadLine());

                Console.Write("Input 2nd Number: ");
                int num2 = int.Parse(Console.ReadLine());

                Console.WriteLine($"Output: {CalculateSum(num1, num2)}");
            }
            Console.ReadLine();
        }
    }
}
