using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;

class GetUserMessageHandler : ServerMessageHandler<GetUserMessage>
{
    protected override IResponse Process(GetUserMessage message, ClientHandler client)
    {
        var target = FindUser(user => user.UID == message.Id, client.CurrentUser, client.Storage);
        if (target is null) return Error(ErrorMessage.UserNotFound);
        return Json(target.GetAsUser());
    }
}
