using System;
using TicketConcession;

namespace TicketConcession
{
    internal class Program
    {
        public static void Main()
        {
            Console.Write("Enter age of person: ");
            int age = int.Parse(Console.ReadLine());
            TicketConcession ticket = new TicketConcession();
            Console.WriteLine(ticket.CalculateConcession(age));
            Console.ReadLine();
        }
    }
}
