using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleMessenger.Core;

public class SMServer
{
    readonly TcpListener _tcpListener = new(IPAddress.Any, 7777);
    readonly List<Task> _newConnection = new();
    readonly IMessageSerializer _serializer = new MessageSerializer();
    readonly IMessageProcessor<ServerMessage> _messageProcessor;

    public SMServer()
    {
        _messageProcessor = new MessageProcessorBuilder<ServerMessage>()
            .Bind<TextMessage>(Console.WriteLine)
            .Build();
    }

    public void Start()
    {
        _tcpListener.Start();
        Console.WriteLine("Server: Started!");

        while (true)
        {
            var tcpClient = _tcpListener.AcceptTcpClient();
            Console.WriteLine($"Server: New connection to server: {tcpClient.Client.RemoteEndPoint}");
            AddNewConnection(tcpClient);
        }
    }

    void AddNewConnection(TcpClient tcpClient)
    {
        Task t = null;

        t = Task.Factory.StartNew(async () =>
        {
            using var stream = tcpClient.GetStream();

            while (tcpClient.Connected)
            {
                try
                {

                    if(!stream.DataAvailable)
                    {
                        Thread.Sleep(1);
                        continue;
                    }
                    var message = await Helper.ReadMessageAsync(stream, _serializer);
                    _messageProcessor.Push(new(message, stream, _serializer));
                }
                catch (Exception ex)
                {
                    if (ex.GetType() == typeof(SocketException))
                    {
                        Console.WriteLine($"{tcpClient.Client.RemoteEndPoint} disconnected");
                        break;
                    }
                    Console.WriteLine(ex.Message);
                }
            }

            _newConnection.Remove(t);
        });
        _newConnection.Add(t);
    }

    class ServerMessage : Message
    {
        public NetworkStream Stream { get; }
        public IMessageSerializer MessageSerializer { get; }

        public ServerMessage(IMessage message, NetworkStream stream, IMessageSerializer messageSerializer) : base(message)
        {
            Stream = stream;
            MessageSerializer = messageSerializer;
        }
    }
}