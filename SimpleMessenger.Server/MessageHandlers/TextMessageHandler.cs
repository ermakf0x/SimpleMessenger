using SimpleMessenger.Core;

namespace SimpleMessenger.Server.MessageHandlers;

class TextMessageHandler : ServerMessageHandlerBase
{
    protected override void Process(IMessage message, ServerClient client)
    {
        if (!IsAuth(message, client))
        {
            ReturnError(client, ErrorMsgHelper.NotAuthorized);
            return;
        }
        Console.WriteLine($"[SERVER]: {message}");
    }
}
