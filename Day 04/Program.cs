using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Day_04
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

            watch.Reset();
            watch.Start();
            var two = PartTwo().Result;
            watch.Stop();

            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
            Console.WriteLine($"Part 2: {two}");
        }

        private static async Task<int> PartOne()
        {
            using var file = System.IO.File.OpenText("input.txt");

            var line = await file.ReadLineAsync();
            if (line == null)
                throw new NullReferenceException();

            var numbers = line.Split(',').Select(int.Parse);

            var boards = new List<Board>();
            await file.ReadLineAsync();
            line = await file.ReadLineAsync();

            while (!string.IsNullOrWhiteSpace(line))
            {
                var x = 0;
                var grid = new BoardPosition[5, 5];

                while (!string.IsNullOrWhiteSpace(line))
                {
                    var y = 0;
                    foreach (var num in line
                        .Split(' ')
                        .Where(static s => !string.IsNullOrWhiteSpace(s))
                        .Select(int.Parse))
                    {
                        grid[x, y] = new BoardPosition
                        {
                            Number = num,
                            Marked = false
                        };

                        y++;
                    }

                    line = await file.ReadLineAsync();
                    x++;
                }

                boards.Add(new Board(grid));

                line = await file.ReadLineAsync();
            }

            Board? winner = null;
            var lastCalled = -1;

            foreach (var call in numbers)
            {
                lastCalled = call;

                foreach (var board in boards)
                {
                    board.Mark(call);

                    if (!board.IsWinner)
                        continue;

                    winner = board;
                    break;
                }

                if (winner != null)
                    break;
            }

            if (winner == null)
                throw new NullReferenceException("No winner");

            var unmarked = winner.Enumerate().Where(static p => !p.Marked).Sum(static p => p.Number);

            return unmarked * lastCalled;
        }

        private static async Task<int> PartTwo()
        {
            using var file = System.IO.File.OpenText("input.txt");

            var line = await file.ReadLineAsync();
            if (line == null)
                throw new NullReferenceException();

            var numbers = line.Split(',').Select(int.Parse);

            var boards = new List<Board>();
            await file.ReadLineAsync();
            line = await file.ReadLineAsync();

            while (!string.IsNullOrWhiteSpace(line))
            {
                var x = 0;
                var grid = new BoardPosition[5, 5];

                while (!string.IsNullOrWhiteSpace(line))
                {
                    var y = 0;
                    foreach (var num in line
                        .Split(' ')
                        .Where(static s => !string.IsNullOrWhiteSpace(s))
                        .Select(int.Parse))
                    {
                        grid[x, y] = new BoardPosition
                        {
                            Number = num,
                            Marked = false
                        };

                        y++;
                    }

                    line = await file.ReadLineAsync();
                    x++;
                }

                boards.Add(new Board(grid));

                line = await file.ReadLineAsync();
            }

            Board? lastWinner = null;
            var lastCalled = -1;

            foreach (var call in numbers)
            {
                lastCalled = call;

                foreach (var board in boards)
                    board.Mark(call);

                if (boards.Count(static b => !b.IsWinner) == 1)
                    lastWinner = boards.Single(static b => !b.IsWinner);

                if (lastWinner is { IsWinner: true })
                    break;
            }

            if (lastWinner == null)
                throw new NullReferenceException("No winner");

            var unmarked = lastWinner.Enumerate().Where(static p => !p.Marked).Sum(static p => p.Number);

            return unmarked * lastCalled;
        }
    }
}