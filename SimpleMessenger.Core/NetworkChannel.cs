using System.Buffers;
using System.Net.Sockets;

namespace SimpleMessenger.Core;

public sealed class NetworkChannel
{
    readonly IMessageSerializer _serializer;
    readonly NetworkStream _stream;

    static readonly int size = sizeof(long);
    static readonly Memory<byte> buffer = new byte[size];

    public bool MessageAvailable => _stream.DataAvailable;

    public NetworkChannel(NetworkStream stream, IMessageSerializer serializer)
    {
        _stream = stream ?? throw new ArgumentNullException(nameof(stream));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    public Task SendAsync(IMessage message)
    {
        if (message is null) throw new ArgumentNullException(nameof(message));

        using var ms = new MemoryStream(256);
        ms.Seek(size, SeekOrigin.Begin);
        _serializer.Serialize(ms, message);
        ms.Position = 0;
        ms.Write(ms.Length);
        ms.Seek(0, SeekOrigin.Begin);
        return ms.CopyToAsync(_stream);
    }
    public async Task<IMessage?> ReceiveAsync()
    {
        if (!_stream.DataAvailable) return null;

        await _stream.ReadAsync(buffer);
        var sizeBlock = BitConverter.ToInt32(buffer.Span) - size;

        using var owner = MemoryPool<byte>.Shared.Rent(sizeBlock);
        var memory = owner.Memory[..sizeBlock];
        var count = await _stream.ReadAsync(memory);
        using var ms = new MemoryStream(memory.ToArray());
        return _serializer.Desirialize(ms);
    }
}
