using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment_2
{
    abstract class Student
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public float Grade { get; set; }

        abstract public bool IsPassed(float grade);
    }

    class UnderGraduate : Student
    {
        public UnderGraduate(string name, int id, float grade)
        {
            Name = name;
            Id = id;
            Grade = grade;
        }
        public override bool IsPassed(float grade)
        {
            return grade > 70.0;
        }
    }

    class Graduate : Student
    {
        public Graduate(string name, int id, float grade)
        {
            Name = name;
            Id = id;
            Grade = grade;
        }

        public override bool IsPassed(float grade)
        {
            return grade > 80.0;
        }
    }

    class Driver
    {
        public static void Main()
        {
            Console.WriteLine("Enter Details for undergraduate student");
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter id: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Enter Grade: ");
            float grade = float.Parse(Console.ReadLine());
            UnderGraduate underGraduate = new UnderGraduate(name, id, grade);
            Console.WriteLine($"UnderGraduate Student Passed: {underGraduate.IsPassed(grade)}");

            Console.WriteLine("\nEnter Details for Graduate Student");
            Console.Write("Enter Name: ");
            name = Console.ReadLine();
            Console.Write("Enter id: ");
            id = int.Parse(Console.ReadLine());
            Console.Write("Enter Grade: ");
            grade = float.Parse(Console.ReadLine());
            Graduate graduate = new Graduate(name, id, grade);
            Console.WriteLine($"Graduate Student Passed: {graduate.IsPassed(grade)}");

            Console.ReadLine();
        }
    }
}
