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
            .Bind<GetUserMessage, GetUserMessageHandler>()
            .Bind<CreateNewChatMessage, CreateNewChatMessageHandler>()
            .Bind<TextSMessage, TextSMessageHandler>()
            .Default(msg => Logger.Warning($"Message type '{msg.GetType().Name}' not supported"))
            .Build();
    }

    public void Run()
    {
        _listener.Start(10);
        Logger.Info("Server started!!");

        while (true)
        {
            if (!_listener.Pending())
            {
                if(_disconnectedQueue.TryDequeue(out var h))
                {
                    Logger.Info($"{h.EndPoint} disconnected from server", ConsoleColor.Blue);
                    _handlers.Remove(h);
                    _connectedUsers.Remove(h.CurrentUser.UID, out _);
                    h.Disconnected -= Handler_Disconnected;
                }

                Thread.Sleep(1);
                continue;
            }

            var newClient = _listener.AcceptTcpClient();
            Logger.Info($"{newClient.Client.RemoteEndPoint} connected to server", ConsoleColor.Green);
            var handler = new ClientHandler(newClient, _serializer, _messageProcessor);
            handler.Disconnected += Handler_Disconnected;
            _handlers.Add(handler);
        }
    }

    public static bool TrySetHandler(User2 user)
    {
        if (_connectedUsers.TryGetValue(user.UID, out var u))
        {
            user.Handler=u.Handler;
            return true;
        }
        return false;
    }
    public static bool TryAddUser(User2 user)
    {
        if(user?.Handler == null) return false;
        return _connectedUsers.TryAdd(user.UID, user);
    }

    void Handler_Disconnected(ClientHandler handler)
    {
        _disconnectedQueue.Enqueue(handler);
    }
}