using SimpleMessenger.Core;
using SimpleMessenger.Server.Model;

namespace SimpleMessenger.Server;

class ServerClient
{
    public NetworkChannel Channel { get; }

    public User2? User { get; set; }

    public ServerClient(NetworkChannel channel)
    {
        Channel = channel ?? throw new ArgumentNullException(nameof(channel));
    }

    public Task SendAsync(IMessage message) => Channel.SendAsync(message);
}