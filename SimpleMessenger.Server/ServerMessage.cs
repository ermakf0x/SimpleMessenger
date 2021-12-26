using SimpleMessenger.Core;
using System.Net.Sockets;

namespace SimpleMessenger.Server;

class ServerMessage : Message
{
    public NetworkStream Stream { get; init; }
    public IMessageSerializer MessageSerializer { get; init; }
}