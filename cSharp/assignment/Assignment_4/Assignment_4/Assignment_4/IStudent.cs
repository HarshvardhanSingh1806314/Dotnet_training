using System;

namespace Assignment_4
{
    internal interface IStudent
    {
        int StudentID { get; set; }
        string Name { get; set; }

        void ShowDetails();
    }

    class DayScholar : IStudent
    {
        public int StudentID { get; set; }
        public String Name { get; set; }
        public void ShowDetails() {
            Console.WriteLine($"Day Scholar Student ID: {StudentID}");
            Console.WriteLine($"Day Scholar Name: {Name}");
        }
    }

    class Resident : IStudent
    { 
        public int StudentID { get; set; }
        public string Name { get; set; }
        public void ShowDetails()
        {
            Console.WriteLine($"Resident Student ID: {StudentID}");
            Console.WriteLine($"Resident Student Name: {Name}");
        }
    }

    class StudentInstantiate
    {
        public static void Main()
        {
            Console.WriteLine("Enter Details for Day Scholar Student");
            Console.Write("Enter Student ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Enter Student Name: ");
            string name = Console.ReadLine();
            DayScholar dayScholar = new DayScholar
            {
                StudentID = id,
                Name = name,
            };
            Console.WriteLine();

            dayScholar.ShowDetails();

            Console.WriteLine("\nEnter Details for Resident Student");
            Console.Write("Enter Student ID: ");
            id = int.Parse(Console.ReadLine());
            Console.Write("Enter Student Name: ");
            name = Console.ReadLine();
            Resident resident = new Resident
            {
                StudentID = id,
                Name = name
            };
            Console.WriteLine();

            resident.ShowDetails();

            Console.ReadLine();
        }
    }
}
