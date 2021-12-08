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

        private static Dictionary<int, List<char>> segments = new Dictionary<int, List<char>>()
        {
            { 1, new List<char>() { 'c', 'f' } },
            { 4, new List<char>() { 'b', 'c', 'd', 'f' } },
            { 7, new List<char>() { 'a', 'c', 'f' } },
            { 8, new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
        };

        private static async Task<long> PartOne()
        {
            using var file = System.IO.File.OpenText("input.txt");

            var line = await file.ReadLineAsync();
            var numEasyDigits = 0;

            while (!string.IsNullOrWhiteSpace(line))
            {
                var parts = line.Split('|');
                var displays = parts[1].Trim().Split(' ');

                numEasyDigits += displays.Count(d => d.Length is 2 or 3 or 4 or 7);

                line = await file.ReadLineAsync();
            }

            return numEasyDigits;
        }

        private static async Task<double> PartTwo()
        {
            using var file = System.IO.File.OpenText("input.txt");

            var line = await file.ReadLineAsync();

            while (!string.IsNullOrWhiteSpace(line))
            {
                var parts = line.Split('|');
                var displays = parts[1].Trim().Split(' ');

                line = await file.ReadLineAsync();
            }

            return -1;
        }
    }
}
