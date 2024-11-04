using System;

namespace Assignment_3
{
    internal class SalesDetails
    {
        private readonly int salesNo;
        private readonly int productNo;
        private readonly double price;
        private readonly DateTime dateOfSale;
        private readonly int quantity;
        private double totalAmount;

        public SalesDetails(int salesNo, int productNo, double price, int quantity, DateTime dateOfSale)
        {
            this.salesNo = salesNo;
            this.productNo = productNo;
            this.price = price;
            this.quantity = quantity;
            this.dateOfSale = dateOfSale;
        }

        public void Sales(int quantity, double price)
        {
            this.totalAmount = quantity * price;
        }

        public static void DisplayDetails(SalesDetails salesDetails) 
        {
            Console.WriteLine("\nDetails of Object");
            Console.WriteLine($"Sales Number: {salesDetails.salesNo}");
            Console.WriteLine($"Product Number: {salesDetails.productNo}");
            Console.WriteLine($"Price: {salesDetails.price}");
            Console.WriteLine($"Quantity: {salesDetails.quantity}");
            Console.WriteLine($"Date Of Sale: {salesDetails.dateOfSale}");
            Console.WriteLine($"Total Amount: {salesDetails.totalAmount}");
        }

        public static void Main()
        {
            Console.WriteLine("Enter Sales Details");

            Console.Write("Enter Sales No.: ");
            int salesNo = int.Parse(Console.ReadLine());

            Console.Write("Enter Product No.: ");
            int productNo = int.Parse(Console.ReadLine());

            Console.Write("Enter Price: ");
            double price = double.Parse(Console.ReadLine());

            Console.Write("Enter Quantity: ");
            int quantity = int.Parse(Console.ReadLine());

            // Creating SalesDetails object
            SalesDetails salesDetails = new SalesDetails(salesNo, productNo, price, quantity, DateTime.Now);

            // calculating total amount of sales
            salesDetails.Sales(quantity, price);

            // Displaying details
            DisplayDetails(salesDetails);

            Console.ReadLine();
        }
    }
}
