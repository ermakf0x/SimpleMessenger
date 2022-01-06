using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;

abstract class ServerMessageHandlerBase : IMessageHandler
{
    public void Process(IMessage message, object? state = null) => Process(message, state as ServerClient ?? throw new ArgumentNullException(nameof(state)));
    protected abstract void Process(IMessage message, ServerClient client);

    protected static void ReturnError(ServerClient client, ErrorMessage message) => client.SendAsync(message);
    protected static void ReturnError(ServerClient client, string message, ErrorMessageType code) => ReturnError(client, new ErrorMessage(message, code));
    protected static void ReturnError(ServerClient client, string message) => ReturnError(client, message, ErrorMessageType.Other);


    protected static bool IsAuth(IMessage message, ServerClient client)
    {
        if (client.User != null) return true;
        if (message is Message msg && msg.Token != Guid.Empty) return true;

        return false;
    }
}
