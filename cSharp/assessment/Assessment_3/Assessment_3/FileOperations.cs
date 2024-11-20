using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment_3
{
    internal class FileOperations
    {
        public static void Main()
        {
            Console.WriteLine("Enter some text to write to the file");
            string fileText = Console.ReadLine();

            Console.WriteLine("Enter filepath");
            string filePath = Console.ReadLine();

            // Appending to the file if it exists otherwise creating a new one and then writing to it
            StreamWriter streamWriter;
            if (File.Exists(filePath))
            {
                Console.WriteLine("Appending");
                streamWriter = new StreamWriter(filePath, true);
                streamWriter.WriteLine(fileText);
            } else
            {
                Console.WriteLine("Creating new");
                streamWriter= new StreamWriter(filePath);
                streamWriter.WriteLine(fileText);
            }

            streamWriter.Close();
            Console.ReadLine();
        }
    }
}
