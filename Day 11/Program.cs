namespace Day_11
{
    public static class Program
    {
        public static async Task Main()
        {
            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();
            var one = await PartOne();
            watch.Stop();

            Console.WriteLine($"Part 1: {one}");
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

            watch.Restart();
            var two = await PartTwo();
            watch.Stop();

            Console.WriteLine($"Part 2: {two}");
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        }

        private static async Task<long> PartOne()
        {
            using var file = File.OpenText("input.txt");

            var line = await file.ReadLineAsync();
            var octopuses = new int[10, 10];

            var x = 0;

            while (!string.IsNullOrWhiteSpace(line))
            {
                for (var y = 0; y < line.Length; y++)
                    octopuses[x, y] = int.Parse(line[y].ToString());

                x++;
                line = await file.ReadLineAsync();
            }

            var flashes = 0;

            for (var step = 1; step <= 100; step++)
            {
                Console.WriteLine($"Part 1, Step {step}");
                // Energy level of each octopus increases by 1
                for (x = 0; x < octopuses.GetLength(0); x++)
                for (var y = 0; y < octopuses.GetLength(1); y++)
                    octopuses[x, y] += 1;

                // Any octopus with an energy over 9 flashes
                bool hasFlashed;
                var flashedThisStep = new List<(int x, int y)>();
                do
                {
                    hasFlashed = false;
                    for (x = 0; x < octopuses.GetLength(0); x++)
                    for (var y = 0; y < octopuses.GetLength(1); y++)
                        if (octopuses[x, y] > 9 && !flashedThisStep.Contains((x, y)))
                        {
                            hasFlashed = true;
                            flashes += 1;
                            flashedThisStep.Add((x, y));

                            for (var dx = -1; dx <= 1; dx++)
                            for (var dy = -1; dy <= 1; dy++)
                                try
                                {
                                    octopuses[x + dx, y + dy] += 1;
                                }
                                catch (IndexOutOfRangeException)
                                {
                                    // Outside the array, don't really care
                                }
                        }
                } while (hasFlashed);

                // any octopus that flashed during this step has its energy level set to 0
                foreach (var (flashedX, flashedY) in flashedThisStep)
                    octopuses[flashedX, flashedY] = 0;
            }

            return flashes;
        }

        private static async Task<long> PartTwo()
        {
            using var file = File.OpenText("input.txt");

            var line = await file.ReadLineAsync();
            var octopuses = new int[10, 10];

            var x = 0;

            while (!string.IsNullOrWhiteSpace(line))
            {
                for (var y = 0; y < line.Length; y++)
                    octopuses[x, y] = int.Parse(line[y].ToString());

                x++;
                line = await file.ReadLineAsync();
            }

            var step = 1;

            while (true)
            {
                Console.WriteLine($"Part 2, Step {step}");
                // Energy level of each octopus increases by 1
                for (x = 0; x < octopuses.GetLength(0); x++)
                for (var y = 0; y < octopuses.GetLength(1); y++)
                    octopuses[x, y] += 1;

                // Any octopus with an energy over 9 flashes
                bool hasFlashed;
                var flashedThisStep = new List<(int x, int y)>();
                do
                {
                    hasFlashed = false;
                    for (x = 0; x < octopuses.GetLength(0); x++)
                    for (var y = 0; y < octopuses.GetLength(1); y++)
                        if (octopuses[x, y] > 9 && !flashedThisStep.Contains((x, y)))
                        {
                            hasFlashed = true;
                            flashedThisStep.Add((x, y));

                            for (var dx = -1; dx <= 1; dx++)
                            for (var dy = -1; dy <= 1; dy++)
                                try
                                {
                                    octopuses[x + dx, y + dy] += 1;
                                }
                                catch (IndexOutOfRangeException)
                                {
                                    // Outside the array, don't really care
                                }
                        }
                } while (hasFlashed);

                // any octopus that flashed during this step has its energy level set to 0
                foreach (var (flashedX, flashedY) in flashedThisStep)
                    octopuses[flashedX, flashedY] = 0;

                if (flashedThisStep.Count == 100)
                    return step;

                step++;
            }
        }
    }
}
