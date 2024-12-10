using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_7
{
    class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string City { get; set; }

        public void Display()
        {
            Console.WriteLine($"Employee ID: {Id}");
            Console.WriteLine($"FirstName: {FirstName}");
            Console.WriteLine($"LastName: {LastName}");
            Console.WriteLine($"Title: {Title}");
            Console.WriteLine($"Date of birth: {DateOfBirth}");
            Console.WriteLine($"Date of joining: {DateOfJoining}");
            Console.WriteLine($"City: {City}\n");
        }
    }
    internal class Program
    {
        static void Main()
        {
            List<Employee> employees = new List<Employee>
            {
                new Employee { 
                    Id = 1001, 
                    FirstName = "Malcolm", 
                    LastName = "Daruwalla", 
                    Title = "Manager", 
                    DateOfBirth = new DateTime(1984, 11, 16), 
                    DateOfJoining = new DateTime(2011, 06, 08), 
                    City = "Mumbai" 
                },
                new Employee { 
                    Id = 1002, 
                    FirstName = "Asdin", 
                    LastName = "Dhalla", 
                    Title = "AsstManager", 
                    DateOfBirth = new DateTime(1994, 08, 20), 
                    DateOfJoining = new DateTime(2012, 07, 07), 
                    City = "Mumbai" 
                },
                new Employee { 
                    Id = 1003, 
                    FirstName = "Madhavi", 
                    LastName = "Oza", 
                    Title = "Consultant", 
                    DateOfBirth = new DateTime(1987, 11, 14), 
                    DateOfJoining = new DateTime(2015, 04, 12), 
                    City = "Pune" 
                },
                new Employee { 
                    Id = 1004, 
                    FirstName = "Saba", 
                    LastName = "Shaikh", 
                    Title = "SE", 
                    DateOfBirth = new DateTime(1990, 06, 03), 
                    DateOfJoining = new DateTime(2016, 02, 02), 
                    City = "Pune" 
                },
                new Employee { 
                    Id = 1005, 
                    FirstName = "Nazia", 
                    LastName = "Shaikh", 
                    Title = "SE", 
                    DateOfBirth = new DateTime(1991, 03, 08), 
                    DateOfJoining = new DateTime(2016, 02, 02), 
                    City = "Mumbai" 
                },
                new Employee { 
                    Id = 1006, 
                    FirstName = "Amit", 
                    LastName = "Pathak", 
                    Title = "Consultant", 
                    DateOfBirth = new DateTime(1989, 11, 07), 
                    DateOfJoining = new DateTime(2014, 08, 08), 
                    City = "Chennai" 
                },
                new Employee { 
                    Id = 1007, 
                    FirstName = "Vijay", 
                    LastName = "Natrajan", 
                    Title = "Consultant", 
                    DateOfBirth = new DateTime(1989, 12, 02), 
                    DateOfJoining = new DateTime(2015, 06, 01), 
                    City = "Mumbai" 
                },
                new Employee { 
                    Id = 1008, 
                    FirstName = "Rahul", 
                    LastName = "Dubey", 
                    Title = "Associate", 
                    DateOfBirth = new DateTime(1993, 11, 11), 
                    DateOfJoining = new DateTime(2014, 11, 06), 
                    City = "Chennai" 
                },
                new Employee { 
                    Id = 1009, 
                    FirstName = "Suresh", 
                    LastName = "Mistry", 
                    Title = "Associate", 
                    DateOfBirth = new DateTime(1992, 08, 12), 
                    DateOfJoining = new DateTime(2014, 12, 03), 
                    City = "Chennai" 
                },
                new Employee { 
                    Id = 1010, 
                    FirstName = "Sumit", 
                    LastName = "Shah", 
                    Title = "Manager", 
                    DateOfBirth = new DateTime(1991, 04, 12), 
                    DateOfJoining = new DateTime(2016, 01, 02), 
                    City = "Pune" 
                }
            };

            // displaying the list of employees who have joined before 1/1/2015
            Console.WriteLine("----------------------Displaying the list of employees who have joined before 1/1/2015----------------------");
            var employeesJoinedBefore = employees.Where(e => e.DateOfJoining < new DateTime(2015, 1, 1));
            foreach(var employee in employeesJoinedBefore)
            {
                employee.Display();
            }

            // displaying the list of employees whose date of birth come after 1/1/1990
            Console.WriteLine("----------------------Displaying the list of employees whose dob is after 1/1/1990----------------------");
            var employeeDOBAfter1190 = employees.Where(e => e.DateOfBirth > new DateTime(1990, 1, 1));
            foreach(var employee in employeeDOBAfter1190)
            {
                employee.Display();
            }

            // displaying the list of employees whose designation is Consultant and Associate
            Console.WriteLine("----------------------Displaying the list of employees whose designation is Consultant and Associate");
            var employeesWithDesignationConsultantAndAssociate = employees.Where(e => e.Title.Equals("Consultant") || e.Title.Equals("Associate"));
            foreach(var employee in employeesWithDesignationConsultantAndAssociate)
            {
                employee.Display();
            }

            // displaying the total no of employees
            Console.WriteLine("----------------------Displaying Total No. of Employees----------------------");
            Console.WriteLine($"Total No. of employees: {employees.Count}");

            // displaying total no of employees belonging to chennai
            Console.WriteLine("----------------------Displaying Total No. of Employees belonging to Chennai----------------------");
            int employeeCountInChennai = employees.Count(e => e.City == "Chennai");
            Console.WriteLine($"Total No. of employees in chennai: {employeeCountInChennai}");

            // displaying highest employee id from the list
            Console.WriteLine("----------------------Displaying Highest Employee Id----------------------");
            int employeeWithHighestId = employees.Max(e => e.Id);
            Console.WriteLine($"Highest Employee ID: {employeeWithHighestId}");

            // displaying total no. of employees who joined after 1/1/2015
            Console.WriteLine("----------------------Displaying Total No. of Employees Who Joined After 1/1/2015");
            int countOfEmployeesJoinedAfter112015 = employees.Count(e => e.DateOfJoining > new DateTime(2015, 1, 1));
            Console.WriteLine($"Total No. of Employees who joined after 1/1/2015: {countOfEmployeesJoinedAfter112015}");

            // displaying total no. of employees whose designation is not Associate
            Console.WriteLine("----------------------Displaying Total No. of Employees whose Designation is not Associate");
            int countOfEmployeesNotAssociate = employees.Count(e => e.Title != "Associate");
            Console.WriteLine($"Total No. of Employees whose Designation is not Associate: {countOfEmployeesNotAssociate}");

            // displaying total no. of employees based on city
            Console.WriteLine("----------------------Displaying Total No. of Employees based on City----------------------");
            var cityAndEmployeeCounts = employees.GroupBy(e => e.City);
            foreach(var group in cityAndEmployeeCounts)
            {
                Console.Write($"{group.Key}: ");
                int count = 0;
                foreach(var employee in group)
                {
                    count++;
                }
                Console.WriteLine($"{count}");
            }

            // displaying total no. of employees based on city and title
            Console.WriteLine("----------------------Displaying Total No. of Employees based on City and Title----------------------");
            var cityAndTitleGroupBy = employees.GroupBy(e => e.City).Select(g => new
            {
                City = g.Key,
                Titles = g.Select(e => e.Title).ToList()
            });
            foreach(var group in cityAndTitleGroupBy)
            {
                Console.Write($"{group.City}: ");
                for (int i= 0;i < group.Titles.Count;i++)
                {  
                    if(i < group.Titles.Count-1)
                    {
                        Console.Write($"{group.Titles[i]}, ");
                    }
                    else
                    {
                        Console.WriteLine(group.Titles[i]);
                    }
                }
            }

            // displaying youngest employees count in the list
            Console.WriteLine("----------------------Displaying Count of Youngest Employees in the list----------------------");
            var youngestEmployee = employees.Select(e => e).Where(e => e.DateOfBirth == employees.Max(emp => emp.DateOfBirth));
            foreach(var employee in youngestEmployee)
            {
                employee.Display();
            }
            Console.ReadLine();
        }
    }
}
