using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Day_07
{
    public static class Program
    {
        public static void Main()
        {
            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();
            var one = PartOne().Result;
            watch.Stop();

            Console.WriteLine($"Part 1: {one}");
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

            watch.Restart();
            var two = PartTwo().Result;
            watch.Stop();

            Console.WriteLine($"Part 2: {two}");
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        }

        private static async Task<long> PartOne()
        {
            using var file = System.IO.File.OpenText("input.txt");

            var line = await file.ReadLineAsync() ?? throw new NullReferenceException();
            List<uint> crabs = line.Split(',').Select(uint.Parse).ToList();

            var bestFuel = long.MaxValue;

            for (var i = 0; i < crabs.Max(); i++)
            {
                var thisFuel = crabs.Sum(crab => Math.Abs(crab - i));

                if (thisFuel < bestFuel)
                    bestFuel = thisFuel;
            }

            return bestFuel;
        }

        private static async Task<double> PartTwo()
        {
            using var file = System.IO.File.OpenText("input.txt");

            var line = await file.ReadLineAsync() ?? throw new NullReferenceException();
            List<uint> crabs = line.Split(',').Select(uint.Parse).ToList();

            var bestFuel = double.MaxValue;

            for (var i = 0; i < crabs.Max(); i++)
            {
                var thisFuel = crabs.Sum(crab =>
                {
                    var move = Math.Abs(crab - i);
                    return move * (move + 1) / 2;
                });

                if (thisFuel < bestFuel)
                    bestFuel = thisFuel;
            }

            return bestFuel;

            // Low: 182695
        }
    }
}
