using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment_2
{
    public delegate int Compare<T>(T x, T y);
    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }

        public void Display()
        {
            Console.WriteLine($"Id: {Id}");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Price: {Price}");
        }
    }

    class SortBasedOnPrice
    {
        public void Sort(Product[] products, Compare<Product> compare)
        {
            for (int i = 0; i < products.Length; i++)
            {
                for (int j = i + 1; j < products.Length; j++)
                {
                    if (compare(products[i], products[j]) > 0)
                    {
                        (products[i], products[j]) = (products[j], products[i]);
                    }
                }
            }
        }

        public static void Main()
        {
            Console.WriteLine("Enter Details for 10 products");
            Product[] products = new Product[10];
            for (int i = 0; i < 10; i++)
            {
                Console.Write("Enter Id: ");
                int id = int.Parse(Console.ReadLine());
                Console.Write("Enter Name: ");
                string name = Console.ReadLine();
                Console.Write("Enter Price: ");
                float price = float.Parse(Console.ReadLine());
                products[i] = new Product { Id = id, Name = name, Price = price };
                Console.WriteLine();
            }

            // Sorting products based on price
            SortBasedOnPrice sort = new SortBasedOnPrice();
            sort.Sort(products, CompareByPrice);

            // Displaying the sorted products
            Console.WriteLine("Sorted Products based on price");
            foreach(Product product in products)
            {
                product.Display();
                Console.WriteLine();
            }

            Console.ReadLine();
        }

        static int CompareByPrice(Product product1, Product product2)
        {
            return product1.Price.CompareTo(product2.Price);
        }
    }
}
