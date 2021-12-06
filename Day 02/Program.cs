// See https://aka.ms/new-console-template for more information

using System;
using System.Threading.Tasks;

namespace Day_2
{
    public static class Program
    {
        public static async Task Main()
        {
            Console.WriteLine("Day 1");
            Console.WriteLine($"Part 1: {await PartOne()}");
            Console.WriteLine($"Part 2: {await PartTwo()}");
        }

        private static async Task<int> PartOne()
        {
            var depth = 0;
            var position = 0;

            using var file = System.IO.File.OpenText("input.txt");

            var line = await file.ReadLineAsync();
            while (!string.IsNullOrWhiteSpace(line))
            {
                var parts = line.Split(' ');

                var command = Enum.Parse<Command>(parts[0], true);
                var amount = int.Parse(parts[1]);

                switch (command)
                {
                    case Command.Forward:
                        position += amount;
                        break;
                    case Command.Down:
                        depth += amount;
                        break;
                    case Command.Up:
                        depth -= amount;
                        break;
                    default:
                        throw new Exception($"Unknown command: {parts[0]}");
                }

                line = await file.ReadLineAsync();
            }

            return depth * position;
        }

        private static async Task<int> PartTwo()
        {
            var depth = 0;
            var position = 0;
            var aim = 0;

            using var file = System.IO.File.OpenText("input.txt");

            var line = await file.ReadLineAsync();
            while (!string.IsNullOrWhiteSpace(line))
            {
                var parts = line.Split(' ');

                var command = Enum.Parse<Command>(parts[0], true);
                var amount = int.Parse(parts[1]);

                switch (command)
                {
                    case Command.Forward:
                        position += amount;
                        depth += aim * amount;
                        break;
                    case Command.Down:
                        aim += amount;
                        break;
                    case Command.Up:
                        aim -= amount;
                        break;
                    default:
                        throw new Exception($"Unknown command: {parts[0]}");
                }

                line = await file.ReadLineAsync();
            }

            return depth * position;
        }
    }
}
