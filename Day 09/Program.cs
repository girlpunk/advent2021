namespace Day_09
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

            var heightMap = new int[100, 100];
            var loadX = 0;

            while (!string.IsNullOrWhiteSpace(line))
            {
                var locations = line.ToCharArray().Select(static c => int.Parse(c.ToString())).ToList();
                for (var y = 0; y < locations.Count; y++)
                    heightMap[loadX, y] = locations[y];

                loadX++;
                line = await file.ReadLineAsync();
            }

            var cache = new List<(int x, int y)>();
            var totalRisk = 0;

            for (var x = 0; x < heightMap.GetLength(0); x++)
            {
                for (var y = 0; y < heightMap.GetLength(1); y++)
                {
                    var oldLowest = heightMap[x, y];
                    var lowest = 10;
                    var checkX = x;
                    var checkY = y;
                    var newX = -1;
                    var newY = -1;

                    do
                    {
                        if (cache.Contains((checkX, checkY)))
                            break;

                        // Check above
                        if (checkX > 0 && heightMap[checkX - 1, checkY] <= lowest)
                        {
                            lowest = heightMap[checkX - 1, checkY];
                            newX = checkX - 1;
                            newY = checkY;
                        }

                        // check below
                        if (checkX < heightMap.GetLength(0) - 1 && heightMap[checkX + 1, checkY] <= lowest)
                        {
                            lowest = heightMap[checkX + 1, checkY];
                            newX = checkX + 1;
                            newY = checkY;
                        }

                        // Check left
                        if (checkY > 0 && heightMap[checkX, checkY - 1] <= lowest)
                        {
                            lowest = heightMap[checkX, checkY - 1];
                            newX = checkX;
                            newY = checkY - 1;
                        }

                        // check right
                        if (checkY < heightMap.GetLength(1) - 1 && heightMap[checkX, checkY + 1] <= lowest)
                        {
                            lowest = heightMap[checkX, checkY + 1];
                            newX = checkX;
                            newY = checkY + 1;
                        }

                        cache.Add((checkX, checkY));

                        if (lowest <= oldLowest)
                        {
                            // continue search
                            checkX = newX;
                            checkY = newY;
                            oldLowest = lowest;
                            lowest = 10;
                        }
                        else
                        {
                            // This is a low point
                            totalRisk += oldLowest + 1;
                            break;
                        }
                    } while (true);
                }
            }

            return totalRisk;
        }

        private static async Task<long> PartTwo()
        {
            using var file = File.OpenText("input.txt");

            var line = await file.ReadLineAsync();

            var heightMap = new int[100, 100];
            var loadX = 0;

            while (!string.IsNullOrWhiteSpace(line))
            {
                var locations = line.ToCharArray().Select(static c => int.Parse(c.ToString())).ToList();
                for (var y = 0; y < locations.Count; y++)
                    heightMap[loadX, y] = locations[y];

                loadX++;
                line = await file.ReadLineAsync();
            }

            var cache = new Dictionary<(int x, int y), (int x, int y)>();
            var basins = new Dictionary<(int x, int y), int>();

            for (var x = 0; x < heightMap.GetLength(0); x++)
            {
                for (var y = 0; y < heightMap.GetLength(1); y++)
                {
                    if (heightMap[x, y] == 9)
                        continue;

                    var oldLowest = heightMap[x, y];
                    var lowest = 10;
                    var checkX = x;
                    var checkY = y;
                    var newX = -1;
                    var newY = -1;
                    var size = 0;
                    var addToCache = new List<(int x, int y)>();

                    do
                    {
                        if (cache.ContainsKey((checkX, checkY)))
                        {
                            basins[cache[(checkX, checkY)]] += size;
                            foreach (var position in addToCache)
                                cache[position] = cache[(checkX, checkY)];
                            break;
                        }

                        // Check above
                        if (checkX > 0 && heightMap[checkX - 1, checkY] <= lowest)
                        {
                            lowest = heightMap[checkX - 1, checkY];
                            newX = checkX - 1;
                            newY = checkY;
                        }

                        // check below
                        if (checkX < heightMap.GetLength(0) - 1 && heightMap[checkX + 1, checkY] <= lowest)
                        {
                            lowest = heightMap[checkX + 1, checkY];
                            newX = checkX + 1;
                            newY = checkY;
                        }

                        // Check left
                        if (checkY > 0 && heightMap[checkX, checkY - 1] <= lowest)
                        {
                            lowest = heightMap[checkX, checkY - 1];
                            newX = checkX;
                            newY = checkY - 1;
                        }

                        // check right
                        if (checkY < heightMap.GetLength(1) - 1 && heightMap[checkX, checkY + 1] <= lowest)
                        {
                            lowest = heightMap[checkX, checkY + 1];
                            newX = checkX;
                            newY = checkY + 1;
                        }

                        if (lowest <= oldLowest)
                        {
                            // continue search
                            size++;
                            addToCache.Add((checkX, checkY));

                            checkX = newX;
                            checkY = newY;
                            oldLowest = lowest;
                            lowest = 10;
                        }
                        else
                        {
                            // This is a low point
                            size++;
                            addToCache.Add((checkX, checkY));

                            basins[(checkX, checkY)] = size;
                            foreach (var position in addToCache)
                                cache[position] = (checkX, checkY);

                            break;
                        }
                    } while (true);
                }
            }

            var largest = basins.Values.OrderByDescending(static v => v).ToArray();

            return largest[0] * largest[1] * largest[2];

            //1435: too high
        }
    }
}
