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
    readonly ConcurrentQueue<IResponse> _messageQueue = new();
    readonly LocalServerConfig _config;
    readonly ManualResetEvent _mre = new(false);
    NetworkChannel? _channel;
    CancellationTokenSource? _cts;
    Task? _workTask;
    IResponse _response;

    public event Action<IMessage>? NewMessageReceived;

    public LocalServer(LocalServerConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public async Task ConnectToServerAsync()
    {
        var client = new TcpClient();
        await client.ConnectAsync(_config.Address, _config.Port).ConfigureAwait(false);
        _channel = new NetworkChannel(client.GetStream(), new MessageSerializer(Encoding.UTF8));
        BeginReceiveMessages();
    }

    public Task<IResponse> SendAsync(IMessage message)
    {
        if (_channel == null) throw new InvalidOperationException();

        return _channel.SendAsync(message).ContinueWith(t =>
        {
            _mre.Reset();
            _mre.WaitOne();
            return _response;
        });
    }
    public void BeginReceiveMessages()
    {
        if(_workTask == null)
        {
            _cts = new CancellationTokenSource();
            _workTask = Task.Factory.StartNew(ReceiveMessageCycleAsync, _cts.Token);
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

        try
        {
            while (!token.IsCancellationRequested)
            {
                var msg = await _channel.ReceiveAsync();
                if (msg != null)
                {
                    if (msg is IResponse res)
                    {
                        _response = res;
                        _mre.Set();
                    }
                    else OnNewMessageReceived(msg);
                }
                Thread.Sleep(1);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    void OnNewMessageReceived(IMessage message)
    {
        if(NewMessageReceived != null)
        {
            App.Current.Dispatcher.Invoke(() => NewMessageReceived.Invoke(message));
        }
    }
}
