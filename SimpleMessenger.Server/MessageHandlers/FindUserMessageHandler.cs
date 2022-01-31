using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;

class FindUserMessageHandler : ServerMessageHandler<FindUserMessage>
{
    protected override IResponse Process(FindUserMessage message, ClientHandler client)
    {
        var target = FindUser(user => user.Username == message.Username, client.CurrentUser, client.Storage);
        if (target == null) return Error(ErrorMessage.UserNotFound);
        return Json(target.GetAsUser());
    }
}