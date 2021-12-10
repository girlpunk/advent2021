using System.ComponentModel.DataAnnotations;

namespace Day_10
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
            var score = 0;

            while (!string.IsNullOrWhiteSpace(line))
            {
                var toFind = "";
                var stop = false;

                foreach (var character in line)
                {
                    switch (character)
                    {
                        case '(':
                            toFind = ")" + toFind;
                            break;
                        case '[':
                            toFind = "]" + toFind;
                            break;
                        case '{':
                            toFind = "}" + toFind;
                            break;
                        case '<':
                            toFind = ">" + toFind;
                            break;
                        case ')':
                            if (toFind[0] == ')')
                            {
                                toFind = toFind[1..];
                            }
                            else
                            {
                                // Corrupted
                                score += 3;
                                stop = true;
                            }

                            break;
                        case ']':
                            if (toFind[0] == ']')
                            {
                                toFind = toFind[1..];
                            }
                            else
                            {
                                // Corrupted
                                score += 57;
                                stop = true;
                            }

                            break;
                        case '}':
                            if (toFind[0] == '}')
                            {
                                toFind = toFind[1..];
                            }
                            else
                            {
                                // Corrupted
                                score += 1197;
                                stop = true;
                            }

                            break;
                        case '>':
                            if (toFind[0] == '>')
                            {
                                toFind = toFind[1..];
                            }
                            else
                            {
                                // Corrupted
                                score += 25137;
                                stop = true;
                            }

                            break;
                        default:
                            throw new ValidationException(character.ToString());
                    }

                    if (stop)
                        break;
                }

                line = await file.ReadLineAsync();
            }

            return score;
        }

        private static async Task<long> PartTwo()
        {
            using var file = File.OpenText("input.txt");

            var line = await file.ReadLineAsync();
            var score = new List<long>();

            while (!string.IsNullOrWhiteSpace(line))
            {
                var toFind = "";
                var stop = false;

                foreach (var character in line)
                {
                    switch (character)
                    {
                        case '(':
                            toFind = ")" + toFind;
                            break;
                        case '[':
                            toFind = "]" + toFind;
                            break;
                        case '{':
                            toFind = "}" + toFind;
                            break;
                        case '<':
                            toFind = ">" + toFind;
                            break;
                        case ')':
                            if (toFind[0] == ')')
                                toFind = toFind[1..];
                            else
                                stop = true;

                            break;
                        case ']':
                            if (toFind[0] == ']')
                                toFind = toFind[1..];
                            else
                                stop = true;

                            break;
                        case '}':
                            if (toFind[0] == '}')
                                toFind = toFind[1..];
                            else
                                stop = true;

                            break;
                        case '>':
                            if (toFind[0] == '>')
                                toFind = toFind[1..];
                            else
                                stop = true;

                            break;
                        default:
                            throw new ValidationException(character.ToString());
                    }

                    if (stop)
                        break;
                }

                if (!stop)
                {
                    var lineScore = 0L;
                    foreach (var character in toFind)
                    {
                        lineScore *= 5;
                        lineScore += character switch
                        {
                            ')' => 1,
                            ']' => 2,
                            '}' => 3,
                            '>' => 4,
                            _ => throw new ValidationException(character.ToString())
                        };
                    }

                    score.Add(lineScore);
                }

                line = await file.ReadLineAsync();
            }

            return score.OrderBy(static s => s).ToList()[(int)Math.Floor(score.Count / 2.0)];

            // Too low
            // 44087106
        }
    }
}
