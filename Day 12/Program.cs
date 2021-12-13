namespace Day_12
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
            var paths = new List<(string start, string end)>();

            while (!string.IsNullOrWhiteSpace(line))
            {
                var parts = line.Split('-');
                paths.Add((start: parts[0], end: parts[1]));
                paths.Add((start: parts[1], end: parts[0]));

                line = await file.ReadLineAsync();
            }

            var possibleRoutes = RecurseRoutes(paths, "start", new List<string>() { "start" }, "start");

            return possibleRoutes;
        }

        private static int RecurseRoutes(ICollection<(string start, string end)> paths,
            string currentCave,
            ICollection<string> visitedCaves,
            string debug)
        {
            var debugCopy = debug + "-" + currentCave;

            if (currentCave == "end")
            {
                // Console.WriteLine(debugCopy);
                return 1;
            }

            var validPaths = paths.Where(path =>
                    path.start == currentCave && (char.IsUpper(path.end[0]) || !visitedCaves.Contains(path.end)))
                .ToList();

            if (validPaths.Count == 0)
                return -1;

            var visitedCopy = visitedCaves.Select(static x => x).ToList();

            if (char.IsLower(currentCave[0]))
                visitedCopy.Add(currentCave);

            var futurePaths = validPaths.Select(path => RecurseRoutes(paths, path.end, visitedCopy, debugCopy))
                .ToList();

            if (futurePaths.All(static x => x == -1))
                return -1;

            return futurePaths.Where(static x => x != -1).Sum();
        }

        private static async Task<long> PartTwo()
        {
            using var file = File.OpenText("input.txt");

            var line = await file.ReadLineAsync();
            var paths = new List<(string start, string end)>();
            var allCaves = new List<string>();

            while (!string.IsNullOrWhiteSpace(line))
            {
                var parts = line.Split('-');
                paths.Add((start: parts[0], end: parts[1]));
                paths.Add((start: parts[1], end: parts[0]));

                if (char.IsLower(parts[0][0]) && !allCaves.Contains(parts[0]))
                    allCaves.Add(parts[0]);

                if (char.IsLower(parts[1][0]) && !allCaves.Contains(parts[1]))
                    allCaves.Add(parts[1]);

                line = await file.ReadLineAsync();
            }

            allCaves.Remove("start");
            allCaves.Remove("end");

            var possibleRoutes = allCaves.SelectMany(allowedTwiceCave =>
                    RecurseRoutes(paths, "start", new List<string>() { "start" }, allowedTwiceCave, false, ""))
                .Distinct()
                .Count();

            return possibleRoutes;
        }

        private static IEnumerable<string> RecurseRoutes(ICollection<(string start, string end)> paths,
            string currentCave,
            ICollection<string> visitedCaves,
            string canVisitTwice,
            bool visitedOnceAlready,
            string route)
        {
            var routeCopy = route + "," + currentCave;

            if (currentCave == "end")
                return new List<string> { routeCopy };

            var validPaths = paths.Where(path =>
                    path.start == currentCave && (char.IsUpper(path.end[0]) || !visitedCaves.Contains(path.end) || (canVisitTwice == path.end && !visitedOnceAlready)))
                .ToList();

            if (validPaths.Count == 0)
                return new List<string>();

            var visitedCopy = visitedCaves.Select(static x => x).ToList();

            if (char.IsLower(currentCave[0]))
                visitedCopy.Add(currentCave);

            return validPaths.SelectMany(path =>
                    RecurseRoutes(
                        paths,
                        path.end,
                        visitedCopy,
                        canVisitTwice,
                        visitedOnceAlready || path.end == canVisitTwice && visitedCaves.Contains(path.end), routeCopy))
                .ToList();
        }
    }
}
