using System.Collections;
using System.Globalization;

namespace Day_16
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

            var line = (await file.ReadLineAsync()).Trim();

            var bitArray = new BitArray(4 * line.Length);
            for (var i = 0; i < line.Length; i++)
            {
                var singleByte = byte.Parse(line[i].ToString(), NumberStyles.HexNumber);
                for (var j = 0; j < 4; j++)
                    bitArray.Set(i * 4 + j, (singleByte & (1 << (3 - j))) != 0);
            }

            var bits = new bool[bitArray.Length];
            bitArray.CopyTo(bits, 0);
            var bitQueue = new Queue<bool>(bits);

            var packet = new Packet(bitQueue);

            return packet.SumVersion;
        }

        private static async Task<long> PartTwo()
        {
            using var file = File.OpenText("input.txt");

            var line = (await file.ReadLineAsync()).Trim();

            var bitArray = new BitArray(4 * line.Length);
            for (var i = 0; i < line.Length; i++)
            {
                var singleByte = byte.Parse(line[i].ToString(), NumberStyles.HexNumber);
                for (var j = 0; j < 4; j++)
                    bitArray.Set(i * 4 + j, (singleByte & (1 << (3 - j))) != 0);
            }

            var bits = new bool[bitArray.Length];
            bitArray.CopyTo(bits, 0);
            var bitQueue = new Queue<bool>(bits);

            var packet = new Packet(bitQueue);

            return packet.Calculate();
        }
    }
}
