using System;
using System.Threading.Tasks;

namespace Day_05
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
            uint[,] vents = new uint[1000, 1000];

            using var file = System.IO.File.OpenText("input.txt");

            var line = await file.ReadLineAsync();

#if DEBUG
            Debug(vents);
#endif
            while (!string.IsNullOrWhiteSpace(line))
            {
                var parts = line.Split(' ');
                var start = parts[0].Split(',');
                var end = parts[2].Split(',');

                var ax = uint.Parse(start[0]);
                var ay = uint.Parse(start[1]);

                var bx = uint.Parse(end[0]);
                var by = uint.Parse(end[1]);

                if (ax != bx && ay != by)
                {
                    line = await file.ReadLineAsync();
                    continue;
                }

                uint startX, endX, startY, endY;

                if (ax < bx)
                {
                    startX = ax;
                    endX = bx;
                }
                else
                {
                    startX = bx;
                    endX = ax;
                }

                if (ay < by)
                {
                    startY = ay;
                    endY = by;
                }
                else
                {
                    startY = by;
                    endY = ay;
                }

                for (var x = startX; x <= endX; x++)
                for (var y = startY; y <= endY; y++)
                    vents[x, y] += 1;

#if DEBUG
                Console.Write(line);
                Debug(vents);
#endif

                line = await file.ReadLineAsync();
            }

            var atLeastTwo = 0;

#if DEBUG
            Debug(vents);
#endif

            for (var x = 0; x < vents.GetLength(0); x++)
            for (var y = 0; y < vents.GetLength(1); y++)
                if (vents[x, y] >= 2)
                    atLeastTwo += 1;

            return atLeastTwo;
        }

        private static void Debug(uint[,] vents)
        {
            for (var x = 0; x < vents.GetLength(0); x++)
            {
                Console.WriteLine();
                for (var y = 0; y < vents.GetLength(1); y++)
                    if (vents[y, x] > 0)
                        Console.Write(vents[y, x]);
                    else
                        Console.Write(".");
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        private static async Task<int> PartTwo()
        {
            uint[,] vents = new uint[1000, 1000];

            using var file = System.IO.File.OpenText("input.txt");

            var line = await file.ReadLineAsync();

            while (!string.IsNullOrWhiteSpace(line))
            {
                var parts = line.Split(' ');
                var start = parts[0].Split(',');
                var end = parts[2].Split(',');

                var ax = uint.Parse(start[0]);
                var ay = uint.Parse(start[1]);

                var bx = uint.Parse(end[0]);
                var by = uint.Parse(end[1]);

                if (ax == bx || ay == by)
                {
                    uint startX, endX, startY, endY;

                    if (ax < bx)
                    {
                        startX = ax;
                        endX = bx;
                    }
                    else
                    {
                        startX = bx;
                        endX = ax;
                    }

                    if (ay < by)
                    {
                        startY = ay;
                        endY = by;
                    }
                    else
                    {
                        startY = by;
                        endY = ay;
                    }

                    for (var x = startX; x <= endX; x++)
                    for (var y = startY; y <= endY; y++)
                        vents[x, y] += 1;
                }
                else
                {
                    if (ax > bx && ay > by)
                    {
                        var i = 0;

                        while (bx + i <= ax)
                        {
                            vents[bx + i, by + i] += 1;
                            i++;
                        }
                    }
                    else if (ax > bx && by > ay)
                    {
                        var i = 0;

                        while (bx + i <= ax)
                        {
                            vents[bx + i, by - i] += 1;
                            i++;
                        }
                    }
                    else if (bx > ax && ay > by)
                    {
                        var i = 0;

                        while (ax + i <= bx)
                        {
                            vents[ax + i, ay - i] += 1;
                            i++;
                        }
                    }
                    else
                    {
                        var i = 0;

                        while (ax + i <= bx)
                        {
                            vents[ax + i, ay + i] += 1;
                            i++;
                        }
                    }
                }

#if DEBUG
                Console.Write(line);
                Debug(vents);
#endif

                line = await file.ReadLineAsync();
            }

            var atLeastTwo = 0;

            for (var x = 0; x < vents.GetLength(0); x++)
            for (var y = 0; y < vents.GetLength(1); y++)
                if (vents[x, y] >= 2)
                    atLeastTwo += 1;

            return atLeastTwo;
        }
    }
}