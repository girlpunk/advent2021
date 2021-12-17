using System.Collections;

namespace Day_16;

public class Packet
{
    public long Version { get; }
    public long PacketType { get; }
    public long? Value { get; }
    public long? LengthType { get; }
    public long? Length { get; }
    public List<Packet>? Subpackets { get; } = new List<Packet>();

    public long Calculate()
    {
        return PacketType switch
        {
            0 => Subpackets?.Sum(static p => p.Calculate()) ?? throw new ArgumentOutOfRangeException(),
            1 => Subpackets?.Aggregate(1L, static (acc, val) => acc * val.Calculate()) ?? throw new ArgumentOutOfRangeException(),
            2 => Subpackets?.Min(static p => p.Calculate()) ?? throw new ArgumentOutOfRangeException(),
            3 => Subpackets?.Max(static p => p.Calculate()) ?? throw new ArgumentOutOfRangeException(),
            4 => Value ?? throw new ArgumentOutOfRangeException(),
            5 => Subpackets?.First().Calculate() > Subpackets?.Skip(1).First().Calculate() ? 1 : 0,
            6 => Subpackets?.First().Calculate() < Subpackets?.Skip(1).First().Calculate() ? 1 : 0,
            7 => Subpackets?.First().Calculate() == Subpackets?.Skip(1).First().Calculate() ? 1 : 0,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public long SumVersion => (Subpackets?.Sum(static packet => packet.SumVersion) ?? throw new ArgumentOutOfRangeException()) + Version;

    public Packet(Queue<bool> data)
    {
        Version = GetIntFromBitArray(new BitArray(DequeueChunk(data, 3).ToArray()));
        PacketType = GetIntFromBitArray(new BitArray(DequeueChunk(data, 3).ToArray()));

        if (PacketType == 4)
        {
            // Literal value
            var val = new List<bool>();

            while (data.Dequeue())
                val.AddRange(DequeueChunk(data, 4));

            val.AddRange(DequeueChunk(data, 4));

            Value = GetIntFromBitArray(new BitArray(val.ToArray()));
        }
        else
        {
            // Operator
            LengthType = GetIntFromBitArray(new BitArray(new[] { data.Dequeue() }));

            if (LengthType == 0)
            {
                // Next 15 are subpacket length
                Length = GetIntFromBitArray(new BitArray(DequeueChunk(data, 15).ToArray()));

                var remaining = data.Count - Length;
                while (data.Count > remaining)
                    Subpackets.Add(new Packet(data));
            }
            else
            {
                // Next 11 are number of subpackets
                Length = GetIntFromBitArray(new BitArray(DequeueChunk(data, 11).ToArray()));

                for (var i = 0; i < Length; i++)
                    Subpackets.Add(new Packet(data));
            }
        }
    }

    private static long GetIntFromBitArray(BitArray bitArray)
    {
        var value = 0L;
        var rev = BitsReverse(bitArray);

        for (var i = 0; i < rev.Count; i++)
            if (rev[i])
                value += Convert.ToInt64(Math.Pow(2, i));

        return value;
    }

    private static BitArray BitsReverse(BitArray bits)
    {
        var len = bits.Count;
        var a = new BitArray(bits);
        var b = new BitArray(bits);

        for (int i = 0, j = len - 1; i < len; ++i, --j)
        {
            a[i] = a[i] ^ b[j];
            b[j] = a[i] ^ b[j];
            a[i] = a[i] ^ b[j];
        }

        return a;
    }

    private static IEnumerable<T> DequeueChunk<T>(Queue<T> queue, int chunkSize)
    {
        for (var i = 0; i < chunkSize && queue.Count > 0; i++)
            yield return queue.Dequeue();
    }
}
