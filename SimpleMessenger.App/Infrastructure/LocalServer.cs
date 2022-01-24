using SimpleMessenger.Core;
using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleMessenger.App.Infrastructure;

class LocalServer
{
    readonly ConcurrentQueue<IMessage> _messageQueue;
    readonly ConcurrentQueue<IResponse> _responseQueue;
    readonly LocalServerConfig _config;
    NetworkChannel? _channel;
    CancellationTokenSource? _cts;
    Task? _workTask;

    public event Action<IMessage>? OnMessage;

    public LocalServer(LocalServerConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));

        _messageQueue = new ConcurrentQueue<IMessage>();
        _responseQueue = new ConcurrentQueue<IResponse>();
    }

    public async Task ConnectToServerAsync()
    {
        var client = new TcpClient();
        await client.ConnectAsync(_config.Address, _config.Port).ConfigureAwait(false);
        _channel = new NetworkChannel(client.GetStream(), new MessageSerializer(Encoding.UTF8));
    }

    public async Task<IResponse> SendAsync(IMessage message)
    {
        if (_channel == null) throw new InvalidOperationException();

        await _channel.SendAsync(message).ConfigureAwait(false);

        while (true)
        {
            if (_responseQueue.TryDequeue(out IResponse res)) return res;

            var msg = await _channel.ReceiveAsync().ConfigureAwait(false);
            if(msg != null)
            {
                if (msg is IResponse res2) return res2;
                _messageQueue.Enqueue(msg);
            }

            Thread.Sleep(1);
        }
    }
    public void BeginReceiveMessages()
    {
        if(_workTask == null)
        {
            _cts = new CancellationTokenSource();
            _workTask = Task.Factory.StartNew(ReceiveMessageCycleAsync, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }
    }
    public void EndReceiveMessages()
    {
        if( _workTask != null && _cts != null )
        {
            _cts.Cancel();
            _cts.Dispose();
            _workTask = null;
            _cts = null;
        }
    }

    async Task ReceiveMessageCycleAsync()
    {
        if (_cts == null) throw new InvalidOperationException();
        var token = _cts.Token;

        while (!token.IsCancellationRequested)
        {
            if(_messageQueue.TryDequeue(out IMessage msg))
            {
                OnMessage?.Invoke(msg);
            }
            else
            {
                var msg2 = await _channel.ReceiveAsync().ConfigureAwait(false);
                if(msg2 != null)
                {
                    if (msg2 is IResponse res)
                        _responseQueue.Enqueue(res);
                    else
                        OnMessage?.Invoke(msg2);
                }
            }
            Thread.Sleep(1);
        }
    }
}
