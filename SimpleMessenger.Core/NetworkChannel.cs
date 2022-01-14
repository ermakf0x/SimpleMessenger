using System.Buffers;
using System.Net.Sockets;

namespace SimpleMessenger.Core;

public sealed class NetworkChannel
{
    readonly IMessageSerializer serializer;
    readonly NetworkStream stream;

    static readonly int size = sizeof(long);
    static readonly byte[] buffer = new byte[size];

    public bool MessageAvailable => stream.DataAvailable;

    public NetworkChannel(NetworkStream stream, IMessageSerializer serializer)
    {
        this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
        this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    public Task SendAsync(IMessage message)
    {
        if (message is null) throw new ArgumentNullException(nameof(message));

        using var ms = new MemoryStream(256);
        ms.Seek(size, SeekOrigin.Begin);
        serializer.Serialize(ms, message);
        ms.Position = 0;
        ms.Write(BitConverter.GetBytes(ms.Length));
        ms.Seek(0, SeekOrigin.Begin);
        return ms.CopyToAsync(stream);
    }
    public async Task<IMessage> ReceiveAsync()
    {
        await stream.ReadAsync(buffer);
        var sizeBlock = BitConverter.ToInt32(buffer, 0) - size;

        using var owner = MemoryPool<byte>.Shared.Rent(sizeBlock);
        var memory = owner.Memory[..sizeBlock];
        var count = await stream.ReadAsync(memory);
        using var ms = new MemoryStream(memory.ToArray());
        return serializer.Desirialize(ms);
    }
}
