using System;

namespace Assignment_4
{
    internal class Scholarship
    {
        public double Merit(double marks, double fees)
        {
            double scholarshipAmount = 0.0;

            if(marks >= 70 && marks <= 80)
            {
                scholarshipAmount = (fees * 20) / 100;
            } else if(marks >80 && marks <= 90)
            {
                scholarshipAmount = (fees * 30) / 100;
            } else if(marks > 90)
            {
                scholarshipAmount = (fees * 50) / 100;
            }

            return scholarshipAmount;
        }
        static void Main(string[] args)
        {
            Scholarship scholarship = new Scholarship();
            Console.WriteLine("Calculating Scholarship Amount");
            Console.Write("Enter Marks: ");
            double marks = double.Parse(Console.ReadLine());
            Console.Write("Enter Fees: ");
            double fees = double.Parse(Console.ReadLine());
            double scholarshipAmount = scholarship.Merit(marks, fees);
            Console.WriteLine($"Scholarship Amount: {scholarshipAmount}");

            Console.ReadLine();
        }
    }
}
