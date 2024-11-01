using System;

namespace Assignment_2
{
	internal class Program
	{
		// function to swap two numbers
		static void Swap(ref int num1, ref int num2)
		{
			num1 += num2;
			num2 = num1 - num2;
			num1 -= num2;
		}

		// function to print pattern for inputted number
		static void PrintPattern(int num)
		{
			for(int i=1;i<=4;i++)
			{
				for(int j=1;j<=4;j++)
				{
					if (i % 2 != 0)
					{
						Console.Write($"{num} ");
					} else
					{
						Console.Write($"{num}");
					}
				}
				Console.WriteLine();
			}
		}

		// function to print day name with respective day no
		static void PrintDayName(int dayNumber)
		{
			Console.Write("Day Name: ");
			switch(dayNumber)
			{
				case 1:
					Console.WriteLine("Monday");
					break;
				case 2:
					Console.WriteLine("Tuesday");
					break;
				case 3:
					Console.WriteLine("Wednesday");
					break;
				case 4:
					Console.WriteLine("Thursday");
					break;
				case 5:
					Console.WriteLine("Friday");
					break;
				case 6:
					Console.WriteLine("Saturday");
					break;
				case 7:
					Console.WriteLine("Sunday");
					break;
				default:
					Console.WriteLine("Invalid Day No.");
					break;
			}
		}

		// function to Caluculate MAX, MIN and AVERAGE values of an array
		static void CalculateMaxMinAndAverage(double[] arr)
		{
			double max = double.MinValue, min = double.MaxValue, sum = 0.0;
			for(int i=0;i<arr.Length;i++)
			{
				sum += arr[i];
				max = Math.Max(max, arr[i]);
				min = Math.Min(min, arr[i]);
			}
			Console.WriteLine($"Maximum Value: {max}, Minimum Value: {min} and Average: {sum / (double)arr.Length}");
		}

		// function to perform calculations on an array containing marks
		static void MarksCalculations(double[] arr)
		{
			double total = 0.0;
			Array.Sort(arr);
			foreach(double element in arr)
			{
				total += element;
			}
			Console.WriteLine($"Total sum of marks: {total}");
			Console.WriteLine($"Average: {total/arr.Length}");
			Console.WriteLine($"Minimum Value: {arr[0]}, Maximum Value: {arr[arr.Length - 1]}");
			Console.Write($"Printing Array in ascending order: ");
			foreach(double element in arr)
			{
				Console.Write($"{element} ");
			}
			Console.WriteLine();
			Console.Write("Printing Array in descending order: ");
			for(int i=arr.Length-1; i>=0;i--)
			{
				Console.Write($"{arr[i]} ");
			}
			Console.WriteLine();
		}

		// function to copy elements from one array to another
		static void CopyArray(int[] arr1, int[] arr2)
		{
			for (int i = 0; i < arr1.Length; i++)
				arr2[i] = arr1[i];
		}
		static void Main(string[] args)
		{
			// input values for the swap function
			{
				Console.WriteLine("Input values for the swap function");
				Console.Write("Enter 1st number: ");
				int num1 = int.Parse(Console.ReadLine());
				Console.Write("Enter 2nd number: ");
				int num2 = int.Parse(Console.ReadLine());
				Console.WriteLine("Values before swapping");
				Console.WriteLine($"num1: {num1}, num2: {num2}");
				Swap(ref num1, ref num2);
				Console.WriteLine("Values after swapping");
				Console.WriteLine($"num1: {num1}, num2: {num2}");
			}

			Console.WriteLine("------------------------------------------------------------------------------");

			// input value for function printing pattern
			{
				Console.WriteLine("input value for function printing pattern");
				Console.Write("Enter the number: ");
				int num = int.Parse(Console.ReadLine());
				PrintPattern(num);
			}

			Console.WriteLine("------------------------------------------------------------------------------");

			// input value for function printing day name
			{
				Console.WriteLine("input value for function printing day name from day number");
				Console.WriteLine("Enter day number: ");
				int dayNumber = int.Parse(Console.ReadLine());
				PrintDayName(dayNumber);
			}

            Console.WriteLine("------------------------------------------------------------------------------");

			// input array for function calculating min, max and average value in it
			{
				Console.WriteLine("input array for function calculating min, max and average value in it");
				Console.Write("Enter the length of array: ");
				int length = int.Parse(Console.ReadLine());
				Console.WriteLine("Enter elements in array");
				double[] arr = new double[length];
				for (int i = 0; i < length; i++)
				{
					arr[i] = double.Parse(Console.ReadLine());
				}
				CalculateMaxMinAndAverage(arr);
			}

			Console.WriteLine("------------------------------------------------------------------------------");

			// input array for MarksCalculation function
			{
				Console.WriteLine("input array for MarksCalculation function");
				Console.WriteLine("Enter elements in array");
				double[] arr = new double[10];
				for (int i = 0; i < 10; i++)
				{
					arr[i] = double.Parse(Console.ReadLine());
				}
				MarksCalculations(arr);
			}

			Console.WriteLine("------------------------------------------------------------------------------");

			// input array for copy function
			{
				Console.WriteLine("input array for copy function");
				Console.Write("Enter length of array: ");
				int length = int.Parse(Console.ReadLine());
				Console.WriteLine("Enter array elements");
				int[] arr1 = new int[length];
				for(int i=0;i<length;i++)
				{
					arr1[i] = int.Parse(Console.ReadLine());
				}
				int[] arr2 = new int[length];
				CopyArray(arr1, arr2);
				Console.WriteLine("Array 2 after copying from Array 1");
				foreach(int element in arr2)
					Console.Write($"{element} ");
			}
            Console.ReadLine();
		}
	}
}
