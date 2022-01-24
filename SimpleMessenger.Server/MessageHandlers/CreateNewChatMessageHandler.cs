using SimpleMessenger.Core;
using SimpleMessenger.Core.Model;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;

class CreateNewChatMessageHandler : ServerMessageHandlerBase<CreateNewChatMessage>
{
    protected override IResponse Process(CreateNewChatMessage message, ClientHandler client)
    {
        if (!message.IsAuth(client)) return Error(ErrorMessage.NotAuthorized);

        var user = LocalDb.GetById(message.Target);
        if (user is null) return Error(ErrorMessage.UserNotFound);

        //TODO: проверить на наличие уже чата и возвращать его

        var chat = new Chat(Guid.NewGuid());

        //TODO: временно для тестов
        user = Server.GetUser(user.UID);
        if (user is not null)
            user.Handler?.SendAsync(new TextMessage(chat.ChatID, client.CurrentUser.UID, message.HelloMessage));


        return Json(chat.ChatID);
    }
}
