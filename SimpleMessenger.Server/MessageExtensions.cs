using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server;

static class MessageExtensions
{
    public static bool IsAuth<TMsg>(this TMsg message, ServerClient client)
        where TMsg : IMessage
    {
        if (client.User != null) return true;
        if (message is Message msg && msg.Token != Token.Empty)
        {
            return LocalDb.ContainsByToken(msg.Token);
        }

        return false;
    }
}