using System.Net.Sockets;

namespace SimpleMessenger.Core;

    public class SMClient
    {
    public string Ip { get; }
    public int Port { get; }
    public bool Connected => _tcpClient != null && _tcpClient.Connected;

    TcpClient _tcpClient;
    NetworkStream _stream;

    public SMClient(string ip, int port)
    {
        Ip = ip;
        Port = port;
    }

    public bool Connect()
        {
        _tcpClient = new TcpClient(Ip, Port);
        _stream = _tcpClient.GetStream();
        return true;
        }

    public void Send(IServerCommand commaned)
        {
        if (!Connected) return;

        var serializer = new CommandSerializer();
        serializer.Serialize(_stream, commaned);
        }
    public IServerCommand Recive()
    {
        if (!Connected) return null;

        using var stream = _tcpClient.GetStream();

        return null;
    }
}
