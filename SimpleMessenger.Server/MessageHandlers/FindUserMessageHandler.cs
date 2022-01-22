using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;

class FindUserMessageHandler : ServerMessageHandlerBase<FindUserMessage>
{
    protected override IResponse Process(FindUserMessage message, ClientHandler client)
    {
        if (!message.IsAuth(client)) return Error(ErrorMessage.NotAuthorized);

        var user = LocalDb.GetByLogin(message.Username);
        if (user is null) return Error(ErrorMessage.UserNotFound);

        return Json(user.GetUser());
    }
}
