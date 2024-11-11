using System;


namespace Assignment_4
{
    internal class Book
    {
        private readonly string _bookName;
        private readonly string _authorName;

        public Book(string bookName, string authorName)
        {
            _bookName = bookName;
            _authorName = authorName;
        }

        public void Display()
        {
            Console.WriteLine($"Book Name: {_bookName}");
            Console.WriteLine($"Author Name: {_authorName}");
        }
    }

    class BookShelf
    {
        private readonly Book[] books = new Book[5];

        public Book this[int index]
        {
            get
            {
                return books[index];
            }

            set
            {
                books[index] = value;
            }
        }

        public static void Main()
        {
            Console.WriteLine("Enter Details of books");
            string bookName, authorName;
            BookShelf bookShelf = new BookShelf();
            for(int i=0;i<5;i++)
            {
                Console.Write($"Enter Book {i + 1} Name: ");
                bookName = Console.ReadLine();
                Console.Write("Enter Author Name: ");
                authorName = Console.ReadLine();
                bookShelf[i] = new Book(bookName, authorName);
                Console.WriteLine();
            }
            Console.Clear();

            // displaying books
            Console.WriteLine("Displaying Books Details");
            Book book;
            for(int i=0;i<5;i++)
            {
                Console.WriteLine($"Book {i + 1} Details");
                book = bookShelf[i];
                book.Display();
                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }
}
