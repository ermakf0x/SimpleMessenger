using SimpleMessenger.App.ViewModel;
using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleMessenger.App.Infrastructure;

sealed class Client : ObservableObject
{
    readonly ManualResetEvent _resetEvent = new(false);
    readonly ClientConfig _config;
    NetworkChannel _channel;
    CancellationTokenSource _cts;
    Task? _workTask;
    IResponse _response;

    public event Action<IMessage>? MessageReceiveEvent;
    public static Client Instance { get; private set; }
    public static MainUser User { get; set; }

    private Client(ClientConfig config)
    {
        _config = config;
    }

    public static Task ConnectAsync(ClientConfig config)
    {
        ArgumentNullException.ThrowIfNull(config, nameof(config));

        //TODO: Необходимо добавить обработку случая когда 'Instance' уже был создан

        Instance = new Client(config);
        return Instance.ConnectAsync().ContinueWith(task =>
        {
            Instance.BeginReceiveMessages();
        });
    }
    public static async Task<bool> BeginSynchronizationAsync(HomeViewModel homeViewModel)
    {
        ArgumentNullException.ThrowIfNull(homeViewModel, nameof(homeViewModel));
        var user = User ?? throw new InvalidOperationException();
        var chats = homeViewModel.Chats;
        Instance.EndReceiveMessages();
        var result = await new Synchronization(Instance._channel, user, chats).BeginSynchronizationAsync();
        Instance.BeginReceiveMessages();
        return result;
    }
    public static Task<IResponse> SendAsync(IMessage message)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));
        return Instance.SendMessageAsync(message);
    }

    async Task ConnectAsync()
    {
        var client = new TcpClient();
        await client.ConnectAsync(_config.Address, _config.Port).ConfigureAwait(false);
        _channel = new NetworkChannel(client.GetStream(), new MessageSerializer(Encoding.UTF8));
    }
    Task<IResponse> SendMessageAsync(IMessage message)
    {
        if (_channel == null) throw new InvalidOperationException();

        return _channel.SendAsync(message).ContinueWith(t =>
        {
            _resetEvent.Reset();
            _resetEvent.WaitOne();
            return _response;
        });
    }
    void BeginReceiveMessages()
    {
        if (_workTask == null)
        {
            _cts = new CancellationTokenSource();
            _workTask = Task.Factory.StartNew(ReceiveMessageCycleAsync, _cts.Token);
        }
    }
    void EndReceiveMessages()
    {
        if (_workTask != null && _cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _workTask = null;
        }
    }
    async Task ReceiveMessageCycleAsync()
    {
        var token = _cts.Token;

        try
        {
            while (!token.IsCancellationRequested)
            {
                var msg = await _channel.ReceiveAsync().ConfigureAwait(false);
                if (msg != null)
                {
                    if (msg is IResponse res)
                    {
                        _response = res;
                        _resetEvent.Set();
                    }
                    else OnMessageReceive(msg);
                }
                await Task.Delay(1, token);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    void OnMessageReceive(IMessage message)
    {
        if (MessageReceiveEvent != null)
        {
            App.Current.Dispatcher.InvokeAsync(() => MessageReceiveEvent.Invoke(message));
        }
    }
}