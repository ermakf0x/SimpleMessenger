using SimpleMessenger.Core;
using SimpleMessenger.Core.Model;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;

sealed class CreateNewChatMessageHandler : ServerMessageHandler<CreateNewChatMessage>
{
    protected override IResponse Process(CreateNewChatMessage message, ClientHandler client)
    {
        var target = FindUser(user => user.UID == message.Target, client.CurrentUser);
        if (target == null) return Error(ErrorMessage.UserNotFound);

        //TODO: проверить на наличие уже чата и возвращать его
        var chat = new Chat(Guid.NewGuid());

        //TODO: временно для тестов
        if (Server.TrySetHandler(target))
            target.Handler.SendAsync(new TextMessage(chat.ChatID, client.CurrentUser.UID, message.Message));

        return Json(chat.ChatID);
    }
}
