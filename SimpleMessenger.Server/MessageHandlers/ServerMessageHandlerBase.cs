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


    protected void Process2(IMessage message, object state)
    {
        if (message is MessageBase msg)
        {
            if (msg.Token == Guid.Empty)
            {
                var client = state as ServerClient;
                client.Channel.SendAsync(ErrorMsgHelper.NotAuthorized);
                return;
            }
        }
    }
}
