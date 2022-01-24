using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;

class TextSMessageHandler : ServerMessageHandlerBase<TextSMessage>
{
    protected override IResponse Process(TextSMessage message, ClientHandler client)
    {
        if (!message.IsAuth(client)) return Error(ErrorMessage.NotAuthorized);

        var user = LocalDb.GetById(message.Target);
        if (user is null) return Error(ErrorMessage.UserNotFound);

        // TODO: временно для тестов
        user = Server.GetUser(user.UID);
        if (user is not null)
            user.Handler.SendAsync(new TextMessage(message.ChatHash, client.CurrentUser.UID, message.Message));

        return Success();
    }
}
