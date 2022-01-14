using System.Net.Sockets;
using System.Text;

namespace SimpleMessenger.Core;

public class SMClient
{
    readonly TcpClient _tcpClient;
    readonly NetworkChannel _channel;

    public bool Connected => _tcpClient != null && _tcpClient.Connected;

    public SMClient(string ip, int port)
    {
        _tcpClient = new TcpClient(ip, port);
        _channel = new NetworkChannel(_tcpClient.GetStream(), new MessageSerializer(Encoding.UTF8));
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
    public ValueTask<IMessage?> ReceiveAsync()
    {
        if (Connected) return _channel.ReceiveAsync();
        return ValueTask.FromResult<IMessage?>(null);
    }
}