using System;
using System.Threading.Tasks;

namespace Day1
{
    public static class Program
    {
        public static async Task Main()
        {
            Console.WriteLine("Day 1");
            Console.WriteLine($"Part 1: {await PartOne()}");
            Console.WriteLine($"Part 2: {await PartTwo()}");
        }

        private static async Task<int> PartOne()
        {
            var increasingLines = 0;
            var previousLine = -1;

            using var file = System.IO.File.OpenText("input.txt");
            var line = await file.ReadLineAsync();

            while (!string.IsNullOrWhiteSpace(line))
            {
                var lineInt = int.Parse(line);

                if (previousLine != -1 && lineInt > previousLine)
                    increasingLines++;

                previousLine = lineInt;
                line = await file.ReadLineAsync();
            }

            return increasingLines;
        }

        private static async Task<int> PartTwo()
        {
            var increasingLines = 0;
            var previousFirst = -1;
            var previousSecond = -1;
            var previousThird = -1;

            using var file = System.IO.File.OpenText("input.txt");
            var line = await file.ReadLineAsync();

            while (!string.IsNullOrWhiteSpace(line))
            {
                var lineInt = int.Parse(line);

                if (previousFirst != -1 && previousSecond != -1 && previousThird != -1 &&
                    lineInt + previousFirst + previousSecond > previousFirst + previousSecond + previousThird)
                    increasingLines++;

                previousThird = previousSecond;
                previousSecond = previousFirst;
                previousFirst = lineInt;

                line = await file.ReadLineAsync();
            }

            return increasingLines;
        }
    }
}
