namespace Day_17
{
    public static class Program
    {
        public static async Task Main()
        {
            var watch = new System.Diagnostics.Stopwatch();

            var one = await PartOne();

            watch.Start();
            await PartOne();
            await PartOne();
            await PartOne();
            await PartOne();
            await PartOne();
            await PartOne();
            await PartOne();
            await PartOne();
            await PartOne();
            await PartOne();
            watch.Stop();

            Console.WriteLine($"Part 1: {one}");
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds / 10} ms");

            var two = await PartTwo();
            watch.Restart();
            await PartTwo();
            await PartTwo();
            await PartTwo();
            await PartTwo();
            await PartTwo();
            await PartTwo();
            await PartTwo();
            await PartTwo();
            await PartTwo();
            await PartTwo();
            watch.Stop();

            Console.WriteLine($"Part 2: {two}");
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds / 10} ms");
        }

        private static async Task<long> PartOne()
        {
            using var file = File.OpenText("input.txt");

            var line = (await file.ReadLineAsync()).Split(' ');
            var x = line[2].Split("..");
            var y = line[3].Split("..");

            var xMin = int.Parse(x[0].Split('=')[1]);
            var xMax = int.Parse(x[1].Replace(",", ""));

            var yMin = int.Parse(y[0].Split('=')[1]);
            var yMax = int.Parse(y[1].Replace(",", ""));

            // var startXVelocity = 0;
            // var startYVelocity = 0;

            var highest = 0;

            for (var startXVelocity = 0; startXVelocity <= xMax; startXVelocity++)
            for (var startYVelocity = 0; startYVelocity <= xMax * 2; startYVelocity++)
            {
                // Console.WriteLine($"{startXVelocity}, {startYVelocity}");

                var thisHighest = 0;
                var xVelocity = startXVelocity;
                var yVelocity = startYVelocity;
                var xPosition = 0;
                var yPosition = 0;

                while (true)
                {
                    // The probe's x position increases by its x velocity.
                    xPosition += xVelocity;

                    // The probe's y position increases by its y velocity.
                    yPosition += yVelocity;

                    // Due to drag, the probe's x velocity changes by 1 toward the value 0
                    if (xVelocity > 0)
                        xVelocity -= 1;
                    else if (xVelocity < 0)
                        xVelocity += 1;

                    // Due to gravity, the probe's y velocity decreases by 1.
                    yVelocity -= 1;

                    if (yPosition > thisHighest)
                        thisHighest = yPosition;

                    // In the target area
                    if (xPosition >= xMin && xPosition <= xMax && yPosition >= yMin && yPosition <= yMax)
                    {
                        if (thisHighest > highest)
                            highest = thisHighest;
                        break;
                    }

                    // Past the target area
                    if (yPosition < yMax || xPosition > xMax)
                        break;
                }
            }

            return highest;

            // Too low
            // 3916
        }

        private static async Task<long> PartTwo()
        {
            using var file = File.OpenText("input.txt");

            var line = (await file.ReadLineAsync()).Split(' ');
            var x = line[2].Split("..");
            var y = line[3].Split("..");

            var xMin = int.Parse(x[0].Split('=')[1]);
            var xMax = int.Parse(x[1].Replace(",", ""));

            var yMin = int.Parse(y[0].Split('=')[1]);
            var yMax = int.Parse(y[1].Replace(",", ""));

            var results = new List<(int xVelocity, int yVelocity)>();

            // var startXVelocity = 0;
            // var startYVelocity = 0;

            for (var startXVelocity = 0; startXVelocity <= xMax; startXVelocity++)
            for (var startYVelocity = -(xMax * 2); startYVelocity <= (xMax * 2); startYVelocity++)
            {
                // Console.WriteLine($"{startXVelocity}, {startYVelocity}");

                var thisHighest = 0;
                var xVelocity = startXVelocity;
                var yVelocity = startYVelocity;
                var xPosition = 0;
                var yPosition = 0;

                while (true)
                {
                    // The probe's x position increases by its x velocity.
                    xPosition += xVelocity;

                    // The probe's y position increases by its y velocity.
                    yPosition += yVelocity;

                    // Due to drag, the probe's x velocity changes by 1 toward the value 0
                    if (xVelocity > 0)
                        xVelocity -= 1;
                    else if (xVelocity < 0)
                        xVelocity += 1;

                    // Due to gravity, the probe's y velocity decreases by 1.
                    yVelocity -= 1;

                    if (yPosition > thisHighest)
                        thisHighest = yPosition;

                    // In the target area
                    if (xPosition >= xMin && xPosition <= xMax && yPosition >= yMin && yPosition <= yMax)
                    {
                        results.Add((xVelocity: startXVelocity, yVelocity: startYVelocity));
                        break;
                    }

                    // Past the target area
                    if (yPosition < yMin || xPosition > xMax)
                        break;
                }
            }

            return results.Count;

            // Too low
            // 3916
        }
    }
}
