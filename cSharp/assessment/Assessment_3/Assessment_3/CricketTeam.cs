using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment_3
{
    internal class CricketTeam
    {
        static Tuple<int, float, int> PointsCalculation(int noOfMatches)
        {
            Console.WriteLine($"Enter score for {noOfMatches} matches");
            int[] matchScores = new int[noOfMatches];
            int sumOfScores = 0;
            float average = 0.0f;
            for (int i = 0; i < noOfMatches; i++)
            {
                Console.Write($"Enter Score of Match {i + 1}: ");
                matchScores[i] = int.Parse(Console.ReadLine());
                sumOfScores += matchScores[i];
            }

            // calculating average score of all matches
            average = sumOfScores / noOfMatches;

            return new Tuple<int, float, int>(noOfMatches, average, sumOfScores);
        }

        static void Main()
        {
            Console.Write("Enter number of matches: ");
            int noOfMatches;
            try
            {
                noOfMatches = int.Parse(Console.ReadLine());
                if (noOfMatches == 0)
                {
                    throw new DivideByZeroException();
                }
                Tuple<int, float, int> result = PointsCalculation(noOfMatches);
                Console.WriteLine($"\nTotal No. of Matches: {result.Item1}");
                Console.WriteLine($"Total score of all matches: {result.Item3}");
                Console.WriteLine($"Average score of {result.Item1} matches: {result.Item2}");
            } catch(DivideByZeroException ex)
            {
                Console.WriteLine("You cannot enter 0 as number of matches.");
            }

            Console.ReadLine();
        }
    }
}
