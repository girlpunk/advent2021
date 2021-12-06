using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Day_03
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
            var bits = line?.Select(static _ => new List<bool>()).ToList() ?? new List<List<bool>>();

            while (!string.IsNullOrWhiteSpace(line))
            {
                for (var i = 0; i < line.Length; i++)
                    bits[i].Add(line[i] == '1');

                line = await file.ReadLineAsync();
            }

            var gammaBits = bits.Select(static i => Math.Abs(Math.Round(i.Average(static b => b ? (decimal)1 : 0)) - 1) == 1)
                .Reverse();

            var gammaArray = new BitArray(gammaBits.Select(static b => b).ToArray());
            var gamma = new byte[2];
            gammaArray.CopyTo(gamma, 0);

            var epsilonString = gammaArray.Not();
            var epsilon = new byte[2];
            epsilonString.CopyTo(epsilon, 0);

            return ((gamma[1] << 8) | gamma[0]) * ((epsilon[1] << 8) | epsilon[0]);
        }

        private static async Task<int> PartTwo()
        {
            var oxygen = new List<List<bool>>();
            var scrubber = new List<List<bool>>();

            using var file = System.IO.File.OpenText("input.txt");
            var line = await file.ReadLineAsync();

            while (!string.IsNullOrWhiteSpace(line))
            {
                oxygen.Add(line.Select(static c => c == '1').ToList());
                scrubber.Add(line.Select(static c => c == '1').ToList());
                line = await file.ReadLineAsync();
            }

            // const string testInput = "00100\n11110\n10110\n10111\n10101\n01111\n00111\n11100\n10000\n11001\n00010\n01010";
            //
            // foreach (var line in testInput.Split('\n'))
            // {
            //     oxygen.Add(line.Select(static c => c == '1').ToList());
            //     scrubber.Add(line.Select(static c => c == '1').ToList());
            // }

            var oxygenPosition = 0;

            while (oxygen.Count > 1)
            {
                var bitsInPosition = oxygen.Select(l => l[oxygenPosition]).ToList();

                var ones = bitsInPosition.Count(static b => b);
                var zeroes = bitsInPosition.Count(static b => b == false);

                oxygen = ones >= zeroes
                    ? oxygen.Where(o => o[oxygenPosition]).ToList()
                    : oxygen.Where(o => o[oxygenPosition] == false).ToList();

                oxygenPosition++;
            }

            var oxygenArray = new BitArray(oxygen.Single().ToArray().Reverse().ToArray());
            var oxygenBytes = new byte[2];
            oxygenArray.CopyTo(oxygenBytes, 0);
            var oxygenValue = (oxygenBytes[1] << 8) | oxygenBytes[0];

            var scrubberPosition = 0;

            while (scrubber.Count > 1)
            {
                var bitsInPosition = scrubber.Select(l => l[scrubberPosition]).ToList();

                var ones = bitsInPosition.Count(static b => b);
                var zeroes = bitsInPosition.Count(static b => b == false);

                scrubber = ones >= zeroes
                    ? scrubber.Where(o => o[scrubberPosition] == false).ToList()
                    : scrubber.Where(o => o[scrubberPosition]).ToList();

                scrubberPosition++;
            }

            var scrubberArray = new BitArray(scrubber.Single().ToArray().Reverse().ToArray());
            var scrubberBytes = new byte[2];
            scrubberArray.CopyTo(scrubberBytes, 0);
            var scrubberValue = (scrubberBytes[1] << 8) | scrubberBytes[0];

            return oxygenValue * scrubberValue;

            // Too low:
            //12147

            // Wrong:
            //3634736
        }
    }
}