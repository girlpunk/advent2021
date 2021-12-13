namespace Day_13
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
            var grid = new bool[895, 1311];

            while (!string.IsNullOrWhiteSpace(line))
            {
                var parts = line.Split(',');
                var x = int.Parse(parts[1]);
                var y = int.Parse(parts[0]);

                grid[x, y] = true;

                line = await file.ReadLineAsync();
            }

            Debug(grid);

            line = await file.ReadLineAsync();

            var foldParts = line.Split(' ');
            foldParts = foldParts[2].Split('=');

            var isHorizontal = foldParts[0] == "y";
            var position = int.Parse(foldParts[1]);

            if (isHorizontal)
            {
                var remainingAway = (grid.GetLength(0) - 1) / 2;
                var remainingAlong = grid.GetLength(1);

                for (var dy = 1; dy <= remainingAway; dy++)
                for (var x = 0; x < remainingAlong; x++)
                {
                    grid[position - dy, x] |= grid[position + dy, x];
                    grid[position + dy, x] = false;
                }
            }
            else
            {
                var remainingAway = (grid.GetLength(1) - 1) / 2;
                var remainingAlong = grid.GetLength(0);

                for (var dx = 1; dx <= remainingAway; dx++)
                for (var y = 0; y < remainingAlong; y++)
                {
                    grid[y, position - dx] |= grid[y, position + dx];
                    grid[y, position + dx] = false;
                }
            }

            Debug(grid);

            return grid.Cast<bool>().Count(static x => x);
        }

        private static void Debug(bool[,] grid, bool force = false)
        {
            if (!force && (grid.GetLength(0) > 50 || grid.GetLength(1) > 50))
                return;

            Console.WriteLine();
            for (var x = 0; x < Math.Min(50, grid.GetLength(0)); x++)
            {
                for (var y = 0; y < Math.Min(50, grid.GetLength(1)); y++)
                    Console.Write(grid[x, y] ? '#' : '.');
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        private static async Task<long> PartTwo()
        {
            using var file = File.OpenText("input.txt");

            var line = await file.ReadLineAsync();
            var grid = new bool[895, 1311];

            while (!string.IsNullOrWhiteSpace(line))
            {
                var parts = line.Split(',');
                var x = int.Parse(parts[1]);
                var y = int.Parse(parts[0]);

                grid[x, y] = true;

                line = await file.ReadLineAsync();
            }

            Debug(grid);

            line = await file.ReadLineAsync();
            while (!string.IsNullOrWhiteSpace(line))
            {
                var foldParts = line.Split(' ');
                foldParts = foldParts[2].Split('=');

                var isHorizontal = foldParts[0] == "y";
                var position = int.Parse(foldParts[1]);

                if (isHorizontal)
                {
                    var remainingAway = (grid.GetLength(0) - 1) / 2;
                    var remainingAlong = grid.GetLength(1);

                    for (var dy = 1; dy <= remainingAway; dy++)
                    for (var x = 0; x < remainingAlong; x++)
                        try
                        {
                            grid[position - dy, x] |= grid[position + dy, x];
                            grid[position + dy, x] = false;
                        }
                        catch (IndexOutOfRangeException)
                        {
                            dy = remainingAway + 1;
                            break;
                        }
                }
                else
                {
                    var remainingAway = (grid.GetLength(1) - 1) / 2;
                    var remainingAlong = grid.GetLength(0);

                    for (var dx = 1; dx <= remainingAway; dx++)
                    for (var y = 0; y < remainingAlong; y++)
                        try
                        {
                            grid[y, position - dx] |= grid[y, position + dx];
                            grid[y, position + dx] = false;
                        }
                        catch (IndexOutOfRangeException)
                        {
                            dx = remainingAway + 1;
                            break;
                        }
                }

                Debug(grid);

                line = await file.ReadLineAsync();
            }

            Debug(grid, true);

            return grid.Cast<bool>().Count(static x => x);
        }
    }
}
