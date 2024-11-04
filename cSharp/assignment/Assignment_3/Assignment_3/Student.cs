using System;

namespace Assignment_3
{
    internal class Student
    {
        private readonly int rollNo;
        private readonly string name;
        private readonly int classNo;
        private readonly int semester;
        private readonly string branch;
        private int[] marks;

        public Student(int rollNo, string name, int classNo, int semester, string branch)
        {
            this.rollNo = rollNo;
            this.name = name;
            this.classNo = classNo;
            this.semester = semester;
            this.branch = branch;
        }

        public void GetMarks()
        {
            this.marks = new int[5];
            Console.WriteLine("\nEnter marks for 5 subjects");
            for(int i=0;i<5;i++)
            {
                Console.Write($"Subject {i + 1}: ");
                marks[i] = int.Parse(Console.ReadLine());
            }
        }

        public void DisplayResult()
        {
            double averageMarks = 0.0;
            Console.Write("\nResult: ");
            foreach(int score in marks)
            {
                if (score > 35)
                    averageMarks += score;
                else
                {
                    Console.WriteLine("Failed");
                    return;
                }
            }
            averageMarks /= 5;

            if (averageMarks < 50)
            {
                Console.WriteLine("Failed");
                return;
            }
            else
                Console.WriteLine("Passed");
        }

        public void DisplayData()
        {
            Console.WriteLine("\nDisplaying student details");
            Console.WriteLine($"Roll No.:{this.rollNo}");
            Console.WriteLine($"Name: {this.name}");
            Console.WriteLine($"Class No.: {this.classNo}");
            Console.WriteLine($"Semester: {this.semester}");
            Console.WriteLine($"Branch: {this.branch}");
            Console.WriteLine("Marks in every subject");
            for(int i=0;i<5;i++)
            {
                Console.WriteLine($"Subject {i + 1}: {this.marks[i]}");
            }
        }

        public static void Main()
        {
            Console.WriteLine("Enter Student Details");

            Console.Write("Enter Roll Number: ");
            int rollNo = int.Parse(Console.ReadLine());

            Console.Write("Enter Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Class No.:");
            int classNo = int.Parse(Console.ReadLine());

            Console.Write("Enter Semester: ");
            int semester = int.Parse(Console.ReadLine());

            Console.Write("Enter Branch: ");
            string branch = Console.ReadLine();

            // instantiating Student Object
            Student student = new Student(rollNo, name, classNo, semester, branch);

            // getting student marks and displaying result
            student.GetMarks();

            // displaying student details
            student.DisplayData();

            // displaying result
            student.DisplayResult();

            Console.ReadLine();
        }
    }
}
