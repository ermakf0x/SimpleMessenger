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
            .Bind<TextMessage, TextMessageHandler>()
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
            var newClient = _listener.AcceptTcpClient();
            Console.WriteLine($"[SERVER] {newClient.Client.RemoteEndPoint} Connected to server");
            _handlers.Add(new ClientHandler(newClient, _serializer, _messageProcessor));
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
}