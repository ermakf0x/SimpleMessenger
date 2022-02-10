using System.Buffers;
using System.Net.Sockets;

namespace SimpleMessenger.Core;

public sealed class NetworkChannel
{
    readonly IMessageSerializer _serializer;
    readonly NetworkStream _stream;
    readonly AsyncLock _receiverLock = new();
    readonly AsyncLock _senderLock = new();

    static readonly int size = sizeof(long);
    static readonly Memory<byte> buffer = new byte[size];

    public bool MessageAvailable => _stream.DataAvailable;
    public bool Connected => _stream.Socket.Connected;
    public Socket Socket => _stream.Socket;

    public NetworkChannel(NetworkStream stream, IMessageSerializer serializer)
    {
        _stream = stream ?? throw new ArgumentNullException(nameof(stream));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    public async Task SendAsync(IMessage message)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        using var ms = new MemoryStream(256);
        ms.Seek(size, SeekOrigin.Begin);
        _serializer.Serialize(ms, message);
        ms.Position = 0;
        ms.Write(ms.Length);
        ms.Seek(0, SeekOrigin.Begin);

        using (await _senderLock.LockAsync())
            await ms.CopyToAsync(_stream);
    }
    public async Task<IMessage?> ReceiveAsync()
    {
        if (!_stream.DataAvailable) return null;

        using(await _receiverLock.LockAsync())
        {
            var c = await _stream.ReadAsync(buffer).ConfigureAwait(false);
            var sizeBlock = BitConverter.ToInt32(buffer.Span) - size;
            using var owner = MemoryPool<byte>.Shared.Rent(sizeBlock);
            var memory = owner.Memory[..sizeBlock];
            var count = await _stream.ReadAsync(memory).ConfigureAwait(false);
            using var ms = new MemoryStream(memory.ToArray());
            return _serializer.Desirialize(ms);
        }
    }

    public override string ToString() => _stream.Socket.RemoteEndPoint?.ToString();
}

class AsyncLock
{
    readonly SemaphoreSlim _semaphore = new(1);
    public async Task<IDisposable> LockAsync()
    {
        var loker = new Locker(_semaphore);
        await loker.LockAsync();
        return loker;
    }
    class Locker : IDisposable
    {
        readonly SemaphoreSlim _semaphore;
        public Locker(SemaphoreSlim semaphore) => _semaphore = semaphore;
        public Task LockAsync() => _semaphore.WaitAsync();
        public void Dispose() => _semaphore.Release();
    }
}