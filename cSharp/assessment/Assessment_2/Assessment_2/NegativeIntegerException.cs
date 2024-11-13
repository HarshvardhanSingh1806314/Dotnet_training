using System;

namespace Assessment_2
{
    class NegativeIntegerException : Exception
    {
        public NegativeIntegerException(string message) : base(message)
        {

        }
    }

    class ExceptionDriver
    {
        static void CheckIfNegative(int value)
        {
            if (value < 0)
            {
                throw new NegativeIntegerException("Entered Integer is negative");
            }
        }
        public static void Main()
        {
            Console.Write("Enter an integer: ");
            int num = int.Parse(Console.ReadLine());
            try
            {
                CheckIfNegative(num);
            } catch(NegativeIntegerException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
