﻿using System.Net.Sockets;
using System.Threading.Tasks;

namespace SimpleMessenger.Core;

public class SMClient
{
    public string Ip { get; }
    public int Port { get; }
    public bool Connected => _tcpClient != null && _tcpClient.Connected;

    TcpClient _tcpClient;
    NetworkStream _stream;
    readonly IMessageSerializer _serializer = new MessageSerializer();

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

    public Task SendAsync(IMessage message)
    {
        if (!Connected) return null;
        return Helper.WriteMessageAsync(_stream, _serializer, message);
    }
    public ValueTask<IMessage> ReciveAsync()
    {
        if (Connected && _stream.DataAvailable)
        {
            return Helper.ReadMessageAsync(_stream, _serializer);
        }
        return ValueTask.FromResult<IMessage>(null);
    }
}