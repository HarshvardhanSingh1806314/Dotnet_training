using System;
using System.IO;

namespace Assignment_4
{
    internal class Employee
    {
        private readonly int _id;
        private readonly string _empName;
        private readonly float _salary;

        public Employee(int id, string empName, float salary = 0)
        {
            _id = id;
            _empName = empName;
            _salary = salary;
        }

        public void Display()
        {
            Console.WriteLine($"Emp ID: {_id}");
            Console.WriteLine($"Emp Name: {_empName}");
            Console.WriteLine($"Emp Salary: {_salary}");
        }
    }

    class PartTimeEmployee : Employee
    {
        private readonly float _wages;

        public PartTimeEmployee(float wages, int id, string empName) : base(id, empName) 
        {
            _wages = wages;
        }

        public new void Display()
        {
            base.Display();
            Console.WriteLine($"Part Time Employee wages: {_wages}");
        }
    }

    class Instantiate
    {
        public static void Main()
        {
            PartTimeEmployee partTimeEmployee = new PartTimeEmployee(40000, 12345, "Durlabh");
            partTimeEmployee.Display();
            Console.ReadLine();
        }
    }
}
