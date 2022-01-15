using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Server.MessageHandlers;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleMessenger.Server;

static class Program
{
    static void Main(string[] args) => new Server().Start();
}

public class Server
{
    readonly TcpListener _tcpListener = new(IPAddress.Any, 7777);
    readonly List<Task> _newConnection = new();
    readonly IMessageSerializer _serializer = new MessageSerializer(Encoding.UTF8);
    readonly IMessageProcessor _messageProcessor;

    public Server()
    {
        _messageProcessor = new MessageProcessorBuilder()
            .Bind<RegistrationMessage, RegistrationMessageHandler>()
            .Bind<AuthorizationMessage, AuthorizationMessageHandler>()
            .Bind<TextMessage, TextMessageHandler>()
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
            var client = new ServerClient(new NetworkChannel(tcpClient.GetStream(), _serializer));

            while (tcpClient.Connected)
            {
                try
                {
                    if(!client.Channel.MessageAvailable)
                    {
                        Thread.Sleep(1);
                        continue;
                    }
                    var newMessage = await client.Channel.ReceiveAsync();
                    _messageProcessor.Push(newMessage, client);
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