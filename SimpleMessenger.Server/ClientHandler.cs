using SimpleMessenger.Core;
using SimpleMessenger.Server.Model;
using System.Net;
using System.Net.Sockets;

namespace SimpleMessenger.Server;

class ClientHandler
{
    readonly IMessageProcessor _messageProcessor;
    readonly NetworkChannel _channel;
    readonly Task _workTask;
    User2? _currentUser;

    public event Action<ClientHandler> Disconnected;

    public EndPoint? EndPoint => _channel.Socket.RemoteEndPoint;
    public User2? CurrentUser
    {
        get => _currentUser;
        set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            if(_currentUser is not null)
            {
                _currentUser.Handler = null;
            }

            _currentUser = value;
            _currentUser.Handler = this;
            Server.TryAddUser(_currentUser);
        }
    }

    public ClientHandler(TcpClient client, IMessageSerializer serializer, IMessageProcessor messageProcessor)
    {
        ArgumentNullException.ThrowIfNull(client, nameof(client));
        ArgumentNullException.ThrowIfNull(serializer, nameof(serializer));
        _messageProcessor = messageProcessor ?? throw new ArgumentNullException(nameof(messageProcessor));

        _channel = new NetworkChannel(client.GetStream(), serializer);
        _workTask = Task.Factory.StartNew(WorkCycleAsync);
    }

    async Task WorkCycleAsync()
    {
        var channel = _channel;

        while (channel.Connected)
        {
            if (!channel.MessageAvailable)
            {
                Thread.Sleep(1);
                continue;
            }

            var message = await channel.ReceiveAsync();
            if (message is not null)
                _messageProcessor.Push(message, this);
        }
        Disconnected?.Invoke(this);
    }

    public Task SendAsync(IMessage message)
    {
        //Logger.Debug($"{EndPoint} ThreadId: {Environment.CurrentManagedThreadId} Message: {message}");
        return _channel.SendAsync(message);
    }

    public override string ToString()
    {
        return _channel.Socket.RemoteEndPoint?.ToString() ?? string.Empty;
    }
}
