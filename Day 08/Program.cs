using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Day_08
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

        private static readonly Dictionary<int, List<char>> _segments = new Dictionary<int, List<char>>()
        {
            { 0, new List<char>() { 'a', 'b', 'c', 'e', 'f', 'g' } },
            { 1, new List<char>() { 'c', 'f' } },
            { 2, new List<char>() { 'a', 'c', 'd', 'e', 'g' } },
            { 3, new List<char>() { 'a', 'c', 'd', 'f', 'g' } },
            { 4, new List<char>() { 'b', 'c', 'd', 'f' } },
            { 5, new List<char>() { 'a', 'b', 'd', 'f', 'g' } },
            { 6, new List<char>() { 'a', 'b', 'd', 'e', 'f', 'g' } },
            { 7, new List<char>() { 'a', 'c', 'f' } },
            { 8, new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
            { 9, new List<char>() { 'a', 'b', 'c', 'd', 'f', 'g' } },
        };

        private static async Task<long> PartOne()
        {
            using var file = System.IO.File.OpenText("input.txt");

            var line = await file.ReadLineAsync();
            var numEasyDigits = 0;

            while (!string.IsNullOrWhiteSpace(line))
            {
                var parts = line.Split('|');
                var displays = parts[1].Trim().Split(' ');

                numEasyDigits += displays.Count(d => d.Length is 2 or 3 or 4 or 7);

                line = await file.ReadLineAsync();
            }

            return numEasyDigits;
        }

        private static async Task<long> PartTwo()
        {
            using var file = File.OpenText("input.txt");

            var line = await file.ReadLineAsync();
            var result = 0;

            while (!string.IsNullOrWhiteSpace(line))
            {
                var possibleSegments = new Dictionary<char, List<char>>()
                {
                    { 'a', new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                    { 'b', new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                    { 'c', new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                    { 'd', new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                    { 'e', new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                    { 'f', new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                    { 'g', new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                };

                var parts = line.Split('|');
                var outputs = parts[1].Trim().Split(' ');

                var displays = outputs.ToList();
                displays.AddRange(parts[0].Trim().Split(' '));

                while (possibleSegments.Any(static p => p.Value.Count > 1))
                {
                    foreach (var display in displays)
                    {
                        if (display.Length == 2)
                        {
                            // 1
                            RemoveOptions(display.ToCharArray().ToList(), _segments[1], possibleSegments);
                        }
                        else if (display.Length == 3)
                        {
                            // 7
                            var remainingSegments = display.ToCharArray().ToList();
                            var existingSegments = possibleSegments
                                .Where(p => remainingSegments.Contains(p.Key) && p.Value.Count == 2).ToList();
                            if (existingSegments.Count == 2)
                            {
                                foreach (var existing in existingSegments)
                                    remainingSegments.Remove(existing.Key);

                                if (remainingSegments.Count == 1)
                                    RemoveOptions(remainingSegments.Single(), 'a', possibleSegments);
                            }
                            else
                            {
                                RemoveOptions(display.ToCharArray().ToList(), _segments[7], possibleSegments);
                            }
                        }
                        else if (display.Length == 4)
                        {
                            // 4
                            RemoveOptions(display.ToCharArray().ToList(), _segments[4], possibleSegments);
                        }
                        else if (display.Length == 5)
                        {
                            // 2, 3, or 5

                            if (possibleSegments.Count(s =>
                                    display.Contains(s.Key) &&
                                    (s.Value.Count <= 2 && (s.Value.Contains('c') && s.Value.Contains('f'))) ||
                                    (s.Value.Count == 1 && s.Value.Contains('c')) ||
                                    (s.Value.Count == 1 && s.Value.Contains('f'))) == 2 &&
                                !possibleSegments.Any(s => display.Contains(s.Key) && s.Value.Count == 1 && (s.Value.Contains('b') || s.Value.Contains('e'))))
                            {
                                // 3
                                RemoveOptions(display.ToCharArray().ToList(), _segments[3], possibleSegments);
                            }

                            if (possibleSegments.Count(s =>
                                    display.Contains(s.Key) &&
                                    s.Value.Count == 1 &&
                                    (
                                        s.Value.Contains('a') ||
                                        s.Value.Contains('d') ||
                                        s.Value.Contains('e') ||
                                        s.Value.Contains('g')
                                    )) == 4 &&
                                !possibleSegments.Any(s => display.Contains(s.Key) && s.Value.Count == 1 && s.Value.Contains('b')) &&
                                possibleSegments.Count(s =>
                                    display.Contains(s.Key) && s.Value.Count <= 2 && s.Value.Contains('c')) == 1)
                            {
                                // 2
                                RemoveOptions(display.ToCharArray().ToList(), _segments[2], possibleSegments);
                            }

                            if (possibleSegments.Count(s =>
                                    display.Contains(s.Key) &&
                                    s.Value.Count == 1 &&
                                    (
                                        s.Value.Contains('a') ||
                                        s.Value.Contains('b') ||
                                        s.Value.Contains('d') ||
                                        s.Value.Contains('g')
                                    )) == 4 &&
                                !possibleSegments.Any(s => display.Contains(s.Key) && s.Value.Count == 1 && s.Value.Contains('e')) &&
                                possibleSegments.Count(s =>
                                    display.Contains(s.Key) && s.Value.Count <= 2 && s.Value.Contains('f')) == 1)
                            {
                                // 5
                                RemoveOptions(display.ToCharArray().ToList(), _segments[5], possibleSegments);
                            }
                        }
                        else if (display.Length == 6)
                        {
                            // 0, 6 or 9

                            if (possibleSegments.Count(s =>
                                    display.Contains(s.Key) &&
                                    (
                                        (s.Value.Count == 1 && s.Value.Contains('e')) ||
                                        (s.Value.Count == 2 && s.Value.Contains('c') && s.Value.Contains('f')) ||
                                        (s.Value.Count == 1 && s.Value.Contains('c')) ||
                                        (s.Value.Count == 1 && s.Value.Contains('f'))
                                    )) == 3 &&
                                !possibleSegments.Any(s =>
                                    display.Contains(s.Key) &&
                                    (
                                        (s.Value.Count == 1 && s.Value.Contains('d'))
                                    )))
                            {
                                // 0
                                RemoveOptions(display.ToCharArray().ToList(), _segments[0], possibleSegments);
                            }

                            if (possibleSegments.Count(s =>
                                    display.Contains(s.Key) &&
                                    (
                                        (s.Value.Count == 1 && s.Value.Contains('a')) ||
                                        (s.Value.Count == 1 && s.Value.Contains('b')) ||
                                        (s.Value.Count == 1 && s.Value.Contains('d')) ||
                                        (s.Value.Count <= 2 && s.Value.Contains('f')) ||
                                        (s.Value.Count == 1 && s.Value.Contains('g'))
                                    )) == 5 &&
                                possibleSegments.Count(s =>
                                    display.Contains(s.Key) &&
                                    (
                                        (s.Value.Count == 1 && s.Value.Contains('e'))
                                    )) == 1 &&
                                possibleSegments.Count(s =>
                                    display.Contains(s.Key) &&
                                    (
                                        (s.Value.Count <= 1 && s.Value.Contains('c')) ||
                                        (s.Value.Count <= 1 && s.Value.Contains('f'))
                                    )) == 1)
                            {
                                // 6
                                RemoveOptions(display.ToCharArray().ToList(), _segments[6], possibleSegments);
                            }

                            if (possibleSegments.Count(s =>
                                    display.Contains(s.Key) &&
                                    (
                                        (s.Value.Count == 1 && s.Value.Contains('d')) ||
                                        (s.Value.Count == 2 && s.Value.Contains('c') && s.Value.Contains('f')) ||
                                        (s.Value.Count == 1 && s.Value.Contains('c')) ||
                                        (s.Value.Count == 1 && s.Value.Contains('f'))
                                    )) == 3)
                            {
                                // 9
                                RemoveOptions(display.ToCharArray().ToList(), _segments[9], possibleSegments);
                            }
                        }
                        else if (display.Length == 7)
                        {
                            // 8, doesn't help us though
                        }
                    }

                    Debug(possibleSegments);
                    // foreach (var (key, value) in possibleSegments.Where(static s => s.Value.Count == 1))
                        // Console.WriteLine($"{key} must be {value.Single()}");
                }

                var output = string.Join(
                    "",
                    outputs.Select(display =>
                    {
                        // 0
                        if (possibleSegments.Count(s =>
                                display.Contains(s.Key) &&
                                (
                                    s.Value.Contains('a') ||
                                    s.Value.Contains('b') ||
                                    s.Value.Contains('c') ||
                                    s.Value.Contains('e') ||
                                    s.Value.Contains('f') ||
                                    s.Value.Contains('g')
                                )) == 6 &&
                            !possibleSegments.Any(s =>
                                display.Contains(s.Key) && s.Value.Contains('d')))
                            return "0";

                        // 1
                        if (possibleSegments.Count(s =>
                                display.Contains(s.Key) &&
                                (
                                    s.Value.Contains('c') ||
                                    s.Value.Contains('f')
                                )) == 2 &&
                            !possibleSegments.Any(s =>
                                display.Contains(s.Key) &&
                                (
                                    s.Value.Contains('a') ||
                                    s.Value.Contains('b') ||
                                    s.Value.Contains('d') ||
                                    s.Value.Contains('e') ||
                                    s.Value.Contains('g')
                                )))
                            return "1";

                        // 2
                        if (possibleSegments.Count(s =>
                                display.Contains(s.Key) &&
                                (
                                    s.Value.Contains('a') ||
                                    s.Value.Contains('c') ||
                                    s.Value.Contains('d') ||
                                    s.Value.Contains('e') ||
                                    s.Value.Contains('g')
                                )) == 5 &&
                            !possibleSegments.Any(s =>
                                display.Contains(s.Key) &&
                                (
                                    s.Value.Contains('b') ||
                                    s.Value.Contains('f')
                                )))
                            return "2";

                        // 3
                        if (possibleSegments.Count(s =>
                                display.Contains(s.Key) &&
                                (
                                    s.Value.Contains('a') ||
                                    s.Value.Contains('c') ||
                                    s.Value.Contains('d') ||
                                    s.Value.Contains('f') ||
                                    s.Value.Contains('g')
                                )) == 5 &&
                            !possibleSegments.Any(s =>
                                display.Contains(s.Key) &&
                                (
                                    s.Value.Contains('b') ||
                                    s.Value.Contains('e')
                                )))
                            return "3";

                        // 4
                        if (possibleSegments.Count(s =>
                                display.Contains(s.Key) &&
                                (
                                    s.Value.Contains('b') ||
                                    s.Value.Contains('c') ||
                                    s.Value.Contains('d') ||
                                    s.Value.Contains('f')
                                )) == 4 &&
                            !possibleSegments.Any(s =>
                                display.Contains(s.Key) &&
                                (
                                    s.Value.Contains('a') ||
                                    s.Value.Contains('e') ||
                                    s.Value.Contains('g')
                                )))
                            return "4";

                        // 5
                        if (possibleSegments.Count(s =>
                                display.Contains(s.Key) &&
                                (
                                    s.Value.Contains('a') ||
                                    s.Value.Contains('b') ||
                                    s.Value.Contains('d') ||
                                    s.Value.Contains('f') ||
                                    s.Value.Contains('g')
                                )) == 5 &&
                            !possibleSegments.Any(s =>
                                display.Contains(s.Key) &&
                                (
                                    s.Value.Contains('c') ||
                                    s.Value.Contains('e')
                                )))
                            return "5";

                        // 6
                        if (possibleSegments.Count(s =>
                                display.Contains(s.Key) &&
                                (
                                    s.Value.Contains('a') ||
                                    s.Value.Contains('b') ||
                                    s.Value.Contains('d') ||
                                    s.Value.Contains('e') ||
                                    s.Value.Contains('f') ||
                                    s.Value.Contains('g')
                                )) == 6 &&
                            !possibleSegments.Any(s =>
                                display.Contains(s.Key) &&
                                (
                                    s.Value.Contains('c')
                                )))
                            return "6";

                        // 7
                        if (possibleSegments.Count(s =>
                                display.Contains(s.Key) &&
                                (
                                    s.Value.Contains('a') ||
                                    s.Value.Contains('c') ||
                                    s.Value.Contains('f')
                                )) == 3 &&
                            !possibleSegments.Any(s =>
                                display.Contains(s.Key) &&
                                (
                                    s.Value.Contains('b') ||
                                    s.Value.Contains('d') ||
                                    s.Value.Contains('e') ||
                                    s.Value.Contains('g')
                                )))
                            return "7";

                        // 8
                        if (possibleSegments.Count(s =>
                                display.Contains(s.Key) &&
                                (
                                    s.Value.Contains('a') ||
                                    s.Value.Contains('b') ||
                                    s.Value.Contains('c') ||
                                    s.Value.Contains('d') ||
                                    s.Value.Contains('e') ||
                                    s.Value.Contains('f') ||
                                    s.Value.Contains('g')
                                )) == 7)
                            return "8";

                        // 9
                        if (possibleSegments.Count(s =>
                                display.Contains(s.Key) &&
                                (
                                    s.Value.Contains('a') ||
                                    s.Value.Contains('b') ||
                                    s.Value.Contains('c') ||
                                    s.Value.Contains('d') ||
                                    s.Value.Contains('f') ||
                                    s.Value.Contains('g')
                                )) == 6 &&
                            !possibleSegments.Any(s =>
                                display.Contains(s.Key) &&
                                (
                                    s.Value.Contains('e')
                                )))
                            return "9";

                        throw new IndexOutOfRangeException();
                    }));

                result += int.Parse(output);

                line = await file.ReadLineAsync();
            }

            return result;
        }

        private static void Debug(IReadOnlyDictionary<char, List<char>> possibilities)
        {
            var a = possibilities.Count(static p => p.Value.Count == 1 && p.Value[0] == 'a') == 1
                ? possibilities.Single(static p => p.Value.Count == 1 && p.Value[0] == 'a').Key.ToString()
                : possibilities.Count(static p => p.Value.Contains('a')).ToString();
            var b = possibilities.Count(static p => p.Value.Count == 1 && p.Value[0] == 'b') == 1
                ? possibilities.Single(static p => p.Value.Count == 1 && p.Value[0] == 'b').Key.ToString()
                : possibilities.Count(static p => p.Value.Contains('b')).ToString();
            var c = possibilities.Count(static p => p.Value.Count == 1 && p.Value[0] == 'c') == 1
                ? possibilities.Single(static p => p.Value.Count == 1 && p.Value[0] == 'c').Key.ToString()
                : possibilities.Count(static p => p.Value.Contains('c')).ToString();
            var d = possibilities.Count(static p => p.Value.Count == 1 && p.Value[0] == 'd') == 1
                ? possibilities.Single(static p => p.Value.Count == 1 && p.Value[0] == 'd').Key.ToString()
                : possibilities.Count(static p => p.Value.Contains('d')).ToString();
            var e = possibilities.Count(static p => p.Value.Count == 1 && p.Value[0] == 'e') == 1
                ? possibilities.Single(static p => p.Value.Count == 1 && p.Value[0] == 'e').Key.ToString()
                : possibilities.Count(static p => p.Value.Contains('e')).ToString();
            var f = possibilities.Count(static p => p.Value.Count == 1 && p.Value[0] == 'f') == 1
                ? possibilities.Single(static p => p.Value.Count == 1 && p.Value[0] == 'f').Key.ToString()
                : possibilities.Count(static p => p.Value.Contains('f')).ToString();
            var g = possibilities.Count(static p => p.Value.Count == 1 && p.Value[0] == 'g') == 1
                ? possibilities.Single(static p => p.Value.Count == 1 && p.Value[0] == 'g').Key.ToString()
                : possibilities.Count(static p => p.Value.Contains('g')).ToString();

            Console.WriteLine($" {a}{a}{a}{a} ");
            Console.WriteLine($"{b}    {c}");
            Console.WriteLine($"{b}    {c}");
            Console.WriteLine($" {d}{d}{d}{d} ");
            Console.WriteLine($"{e}    {f}");
            Console.WriteLine($"{e}    {f}");
            Console.WriteLine($" {g}{g}{g}{g} ");
        }

        private static readonly List<char> AllSegments = new List<char>()
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g'
        };

        private static void RemoveOptions(char segments, char toKeep, IReadOnlyDictionary<char, List<char>> possibleSegments)
        {
            RemoveOptions(new List<char> { segments }, new List<char> { toKeep }, possibleSegments);
        }

        private static void RemoveOptions(ICollection<char> segments, ICollection<char> toKeep, IReadOnlyDictionary<char, List<char>> possibilities)
        {
            foreach (var segment in segments)
            foreach (var possibility in possibilities[segment].Where(p => !toKeep.Contains(p)).ToList())
                possibilities[segment].Remove(possibility);

            foreach (var segment in AllSegments.Where(s => !segments.Contains(s)))
            foreach (var possibility in possibilities[segment].Where(toKeep.Contains).ToList())
                possibilities[segment].Remove(possibility);
        }
    }
}
