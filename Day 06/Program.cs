using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Day_06
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

        private static async Task<int> PartOne()
        {
            const int days = 80;
            const int respawn = 6;
            const int startup = 2;

            using var file = System.IO.File.OpenText("input.txt");

            var line = await file.ReadLineAsync() ?? throw new NullReferenceException();
            List<int> fish = line.Split(',').Select(int.Parse).ToList();

            for (var i = 0; i < days; i++)
            {
                var newFish = fish.Where(static f => f == 0).Select(static _ => respawn + startup).ToList();
                newFish.AddRange(fish.Select(static f => f == 0 ? respawn : f - 1));

                fish = newFish;
            }

            return fish.Count;
        }

        private static async Task<ulong> PartTwo()
        {
            const int days = 256;

            using var file = System.IO.File.OpenText("input.txt");

            var line = await file.ReadLineAsync() ?? throw new NullReferenceException();

            var fish = line
                .Split(',')
                .Select(int.Parse)
                .GroupBy(static f => f)
                .ToDictionary(static f => f.Key, static f => (ulong)f.Count());

            for (var i = 0; i < days; i++)
            {
                Console.WriteLine(i);

                var newFish = new Dictionary<int, ulong>
                {
                    { 0, fish.GetValueOrDefault(1, 0u) },
                    { 1, fish.GetValueOrDefault(2, 0u) },
                    { 2, fish.GetValueOrDefault(3, 0u) },
                    { 3, fish.GetValueOrDefault(4, 0u) },
                    { 4, fish.GetValueOrDefault(5, 0u) },
                    { 5, fish.GetValueOrDefault(6, 0u) },
                    { 6, fish.GetValueOrDefault(7, 0u) + fish.GetValueOrDefault(0, 0u) },
                    { 7, fish.GetValueOrDefault(8, 0u) },
                    { 8, fish.GetValueOrDefault(0, 0u) }
                };

                fish = newFish;
            }

            return fish.Aggregate((ulong)0, static (prev, pair) => prev + pair.Value);
        }
    }
}
