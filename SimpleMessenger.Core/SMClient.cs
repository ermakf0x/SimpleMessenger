using System.Net.Sockets;
using System.Threading.Tasks;

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

    public Task SendAsync(IMessage message)
    {
        if (!Connected) return null;
        return _channel.SendAsync(message);
    }
    public ValueTask<IMessage> ReceiveAsync()
    {
        if (Connected && _channel.MessageAvailable)
            return _channel.ReceiveAsync();
        return ValueTask.FromResult<IMessage>(null);
    }
}
