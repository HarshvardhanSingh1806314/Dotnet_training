using System;

namespace Assignment_4
{
    internal class Doctor
    {
        private long _regNo;
        private string _name;
        private float _feesCharged;

        public long RegNo { 
            get
            {
                return _regNo;
            }

             set
             {
                _regNo = value;
             }
        }
        public string Name {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }
        public float FeesCharged { 
            get
            {
                return _feesCharged;
            }

            set
            {
                _feesCharged = value;
            }
        }
        
        public static void Main()
        {
            Console.WriteLine("Enter Doctor Details");
            Console.Write("Enter Registration No.: ");
            long regNo = long.Parse(Console.ReadLine());
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Doctor Fees: ");
            float feesCharged = float.Parse(Console.ReadLine());
            Doctor doctor = new Doctor
            {
                RegNo = regNo,
                Name = name,
                FeesCharged = feesCharged
            };

            // displaying the doctor information
            Console.WriteLine("\nDisplaying Doctor Details");
            Console.WriteLine($"Registration No.: {doctor.RegNo}");
            Console.WriteLine($"Doctor Name: {doctor.Name}");
            Console.WriteLine($"Fees Charged: {doctor.FeesCharged}");

            Console.ReadLine();
        }
    }
}
