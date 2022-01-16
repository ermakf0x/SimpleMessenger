using SimpleMessenger.Core.Messages;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleMessenger.Core;

public sealed class SMClient
{
    readonly TcpClient _tcpClient;
    readonly IPEndPoint _endPoint;
    NetworkChannel _channel;

    public bool Connected => _tcpClient.Connected;

    public SMClient(string ip, int port)
    {
        _tcpClient = new TcpClient();
        _endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
    }
    ~SMClient()
    {
        if(Connected) _tcpClient.Dispose();
    }

    public async Task ConnectAsync()
    {
        await _tcpClient.ConnectAsync(_endPoint).ConfigureAwait(false);
        _channel = new NetworkChannel(_tcpClient.GetStream(), new MessageSerializer(Encoding.UTF8));
    }
    public async Task<IResponse> ConnectAsync(HelloServerMessage helloMessage)
    {
        ArgumentNullException.ThrowIfNull(helloMessage, nameof(helloMessage));

        await _tcpClient.ConnectAsync(_endPoint).ConfigureAwait(false);
        _channel = new NetworkChannel(_tcpClient.GetStream(), new MessageSerializer(Encoding.UTF8));

        return await SendAsync(helloMessage).ConfigureAwait(false);
    }

    public async Task<IResponse> SendAsync(IMessage message)
    {
        if (!Connected) return null; // TODO: Если нет соединения - бросать исключение

        await _channel.SendAsync(message).ConfigureAwait(false);
        while (true)
        {
            if (!_channel.MessageAvailable)
            {
                Thread.Sleep(1);
                continue;
            }
            return await _channel.ReceiveAsync().ConfigureAwait(false) as IResponse;
        }
    }
    public Task<IMessage?> ReceiveAsync()
    {
        if (Connected) return _channel.ReceiveAsync();
        return Task.FromResult<IMessage?>(null);
    }
}