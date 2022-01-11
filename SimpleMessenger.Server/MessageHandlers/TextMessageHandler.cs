using SimpleMessenger.Core;

namespace SimpleMessenger.Server.MessageHandlers;

class TextMessageHandler : ServerMessageHandlerBase
{
    protected override IResponse Process(IMessage message, ServerClient client)
    {
        if (!message.IsAuth(client)) return Error(ErrorMsgHelper.NotAuthorized);

        Console.WriteLine($"[SERVER]: {message}");
        return Success();
    }
}
