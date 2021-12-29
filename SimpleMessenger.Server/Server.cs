using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Server.MessageHandlers;
using System.Net;
using System.Net.Sockets;

namespace SimpleMessenger.Server;

public class Server
{
    readonly TcpListener _tcpListener = new(IPAddress.Any, 7777);
    readonly List<Task> _newConnection = new();
    readonly IMessageSerializer _serializer = new MessageSerializer();
    readonly IMessageProcessor<ServerClient> _messageProcessor;

    public Server()
    {
        _messageProcessor = new MessageProcessorBuilder<ServerClient>()
            .Bind<AuthorizationMessage, AuthMessageHandler>()
            .Bind2<TextMessage>(text => Console.WriteLine($"[SERVER] {text}"))
            .Bind2<GetUsersMessage>(new GetUsersHandler())
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
        Task? t = null;

        t = Task.Factory.StartNew(async () =>
        {
            using var stream = tcpClient.GetStream();
            var client = new ServerClient
            {
                Stream = stream,
                MessageSerializer = _serializer
            };

            while (tcpClient.Connected)
            {
                try
                {
                    if(!stream.DataAvailable)
                    {
                        Thread.Sleep(1);
                        continue;
                    }
                    var newMessage = await Helper.ReadMessageAsync(stream, _serializer);
                    client.MSG = newMessage;
                    _messageProcessor.Push(client);
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
}