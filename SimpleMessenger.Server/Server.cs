using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Server.MessageHandlers;
using SimpleMessenger.Server.Model;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleMessenger.Server;

class Server
{
    readonly TcpListener _listener = new(IPAddress.Any, 7777);
    readonly List<ClientHandler> _handlers = new();
    readonly ConcurrentQueue<ClientHandler> _disconnectedQueue = new();
    readonly IMessageSerializer _serializer = new MessageSerializer(Encoding.UTF8);
    readonly IMessageProcessor _messageProcessor;

    readonly static ConcurrentDictionary<int, User2> _connectedUsers = new();

    readonly static Lazy<Server> _instance = new(() => new Server(), true);
    public static Server Instance => _instance.Value;

    Server()
    {
        _messageProcessor = new MessageProcessorBuilder()
            .Bind<HelloServerMessage, HelloServerMessageHandler>()
            .Bind<RegistrationMessage, RegistrationMessageHandler>()
            .Bind<AuthorizationMessage, AuthorizationMessageHandler>()
            .Bind<FindUserMessage, FindUserMessageHandler>()
            .Bind<TextSMessage, TextSMessageHandler>()
            .Bind<CreateNewChatMessage, CreateNewChatMessageHandler>()
            .Default(msg => Console.WriteLine($"[SERVER] WARNING message type '{msg.GetType().Name}' not supported"))
            .Build();
    }

    public void Run()
    {
        _listener.Start(10);
        Console.WriteLine("[SERVER] Started!!");

        while (true)
        {
            if (!_listener.Pending())
            {
                if(_disconnectedQueue.TryDequeue(out var h))
                {
                    _handlers.Remove(h);
                    h.Disconnected -= Handler_Disconnected;
                }

                Thread.Sleep(1);
                continue;
            }

            var newClient = _listener.AcceptTcpClient();
            Console.WriteLine($"[SERVER] {newClient.Client.RemoteEndPoint} Connected to server");
            var handler = new ClientHandler(newClient, _serializer, _messageProcessor);
            handler.Disconnected += Handler_Disconnected;
            _handlers.Add(handler);
        }
    }

    public static User2? GetUser(int uid)
    {
        if (_connectedUsers.TryGetValue(uid, out var user)) return user;
        return null;
    }
    public static bool TryAddUser(User2 user)
    {
        if(user == null) return false;
        return _connectedUsers.TryAdd(user.UID, user);
    }

    void Handler_Disconnected(ClientHandler handler)
    {
        _disconnectedQueue.Enqueue(handler);
    }
}