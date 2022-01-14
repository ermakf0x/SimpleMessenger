using System.Net.Sockets;

namespace SimpleMessenger.Core;

public class SMClient
{
    public bool Connected => _tcpClient != null && _tcpClient.Connected;

    readonly TcpClient _tcpClient;
    readonly NetworkChannel _channel;

    public SMClient(string ip, int port)
    {
        _tcpClient = new TcpClient(ip, port);
        _channel = new NetworkChannel(_tcpClient.GetStream(), new MessageSerializer());
    }

    public async Task<IResponse> SendAsync(IMessage message)
    {
        if (!Connected) return null; // TODO: Если нет соединения - бросать исключение

        await _channel.SendAsync(message);
        while (true)
        {
            if (!_channel.MessageAvailable)
            {
                Thread.Sleep(1);
                continue;
            }
            return await _channel.ReceiveAsync() as IResponse;
        }
    }
    public Task<IMessage> ReceiveAsync()
    {
        if (Connected && _channel.MessageAvailable)
            return _channel.ReceiveAsync();
        return Task.FromResult<IMessage>(null);
    }
}