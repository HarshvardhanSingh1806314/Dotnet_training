using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment_6
{
    internal class EmployessQuery
    {
        private delegate int Compare(EmployessQuery emp1, EmployessQuery emp2);
        private int _empId;
        private string _empName;
        private string _empCity;
        private float _empSalary;

        public EmployessQuery(int empId, string empName, string empCity, float empSalary)
        {
            _empId = empId;
            _empName = empName;
            _empCity = empCity;
            _empSalary = empSalary;
        }

        static void Sort(EmployessQuery[] employees, Compare compareByName)
        {
            for(int i=0;i<employees.Length;i++)
            {
                for(int j=i+1;j<employees.Length;j++)
                {
                    if (compareByName(employees[i], employees[j]) > 0)
                    {
                        (employees[i], employees[j]) = (employees[j], employees[i]);
                    }
                }
            }
        }

        static int CompareByName(EmployessQuery emp1, EmployessQuery emp2)
        {
            return emp1._empName.CompareTo(emp2._empName);
        }

        public void Display()
        {
            Console.WriteLine($"Id: {_empId}");
            Console.WriteLine($"Name: {_empName}");
            Console.WriteLine($"City: {_empCity}");
            Console.WriteLine($"Salary: {_empSalary}");
        }
        public static void Main()
        {
            Console.Write("Enter Number of Employees: ");
            int n = int.Parse(Console.ReadLine());
            EmployessQuery[] employees = new EmployessQuery[n];
            Console.WriteLine("Enter Employees details");
            for(int i=0;i<n;i++)
            {
                Console.Write($"Enter Emp {i + 1} ID: ");
                int id = int.Parse(Console.ReadLine());
                Console.Write($"Enter Emp {i + 1} Name: ");
                string name = Console.ReadLine();
                Console.Write($"Enter Emp {i + 1} City: ");
                string city = Console.ReadLine();
                Console.Write($"Enter Emp {i + 1} Salary: ");
                float salary = float.Parse(Console.ReadLine());
                employees[i] = new EmployessQuery(id, name, city, salary);
                Console.WriteLine();
            }

            // printing details of every employee
            Console.Clear();
            Console.WriteLine("Printing all employees details");
            for(int i=0;i<n;i++)
            {
                Console.WriteLine($"Details of Employee {i + 1}");
                employees[i].Display();
                Console.WriteLine();
            }

            // printing details of employees with salary > 45000
            Console.WriteLine("\nPrinting details of employees with salary > 45000");
            IEnumerable<EmployessQuery> list = employees.Where<EmployessQuery>(emp => emp._empSalary > 45000);
            foreach (EmployessQuery emp in list)
            {
                emp.Display();
                Console.WriteLine();
            }

            // printing details of employees who belong to bangalore
            Console.WriteLine("\nPrinting details of employees who belong to bangalore");
            list = employees.Where<EmployessQuery>(emp => emp._empCity.Equals("Bangalore"));
            foreach(EmployessQuery emp in list)
            {
                emp.Display();
                Console.WriteLine();
            }

            // printing details of employees sorted based on name
            Sort(employees, CompareByName);
            Console.WriteLine("\nEmployees sorted based on their names");
            foreach(EmployessQuery emp in employees)
            {
                emp.Display();
                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }
}
