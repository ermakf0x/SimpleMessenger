using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server;

static class MessageExtensions
{
    public static bool IsAuth(this IMessage message, ServerClient client)
    {
        if (client.User != null) return true;
        if (message is Message msg && msg.Token != Token.Empty) return true;

        return false;
    }
}