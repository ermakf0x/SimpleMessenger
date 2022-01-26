using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;

class TextSMessageHandler : ServerMessageHandler<TextSMessage>
{
    protected override IResponse Process(TextSMessage message, ClientHandler client)
    {
        var target = FindUser(user => user.UID == message.Target, client.CurrentUser);
        if (target == null) return Error(ErrorMessage.UserNotFound);

        // TODO: временно для тестов
        if (Server.TrySetHandler(target))
            target.Handler.SendAsync(new TextMessage(message.ChatID, client.CurrentUser.UID, message.Message));

        return Success();
    }
}
