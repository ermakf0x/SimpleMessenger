using SimpleMessenger.Core;
using System.Net.Sockets;

namespace SimpleMessenger.Server;

class ServerClient : Message
{
    public NetworkStream? Stream { get; init; }
    public IMessageSerializer? MessageSerializer { get; init; }
    public User2? User { get; set; }
}