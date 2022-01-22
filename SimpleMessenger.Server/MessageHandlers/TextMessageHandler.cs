using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Server.MessageHandlers;

class TextMessageHandler : ServerMessageHandlerBase<TextMessage>
{
    protected override IResponse Process(TextMessage message, ClientHandler client)
    {
        if (!message.IsAuth(client)) return Error(ErrorMessage.NotAuthorized);

        var user = LocalDb.GetById(message.Target);
        if (user is null) return Error(ErrorMessage.UserNotFound);

        // TODO: временно для тестов
        user = Server.GetUser(user.UID);
        if (user is not null)
            user.Handler.SendAsync(new TextMessage(Token.Empty, message.ChatHash, user.UID, message.Message));

        return Success();
    }
}
