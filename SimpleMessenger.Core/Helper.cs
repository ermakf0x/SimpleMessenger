using System;
using System.Buffers;
using System.IO;
using System.Threading.Tasks;

namespace SimpleMessenger.Core;

internal static class Helper
{
    const int SIZE = sizeof(long);
    static readonly byte[] buffer = new byte[SIZE];

    public static Task WriteMessageAsync(Stream stream, IMessageSerializer serializer, IMessage command)
    {
        using var ms = new MemoryStream(256);
        ms.Seek(sizeof(long), SeekOrigin.Begin);
        serializer.Serialize(ms, command);
        ms.Position = 0;
        ms.Write(BitConverter.GetBytes(ms.Length));
        ms.Seek(0, SeekOrigin.Begin);
        return ms.CopyToAsync(stream);
    }

    public static async ValueTask<IMessage> ReadMessageAsync(Stream stream, IMessageSerializer serializer)
    {
        await stream.ReadAsync(buffer);
        var sizeBlock = BitConverter.ToInt32(buffer, 0) - SIZE;
        using var memoryOwner = MemoryPool<byte>.Shared.Rent(sizeBlock);
        var memory = memoryOwner.Memory[..sizeBlock];
        var count = await stream.ReadAsync(memory);
        using var ms = new MemoryStream(memory.ToArray());
        return serializer.Desirialize(ms);
    }
}