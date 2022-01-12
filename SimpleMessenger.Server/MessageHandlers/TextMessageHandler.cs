using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;

class TextMessageHandler : ServerMessageHandlerBase<TextMessage>
{
    protected override IResponse Process(TextMessage message, ServerClient client)
    {
        if (!message.IsAuth(client)) return Error(ErrorMsgHelper.NotAuthorized);

        Console.WriteLine($"[SERVER]: {message}");
        return Success();
    }
}
