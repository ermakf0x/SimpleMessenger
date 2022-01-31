using SimpleMessenger.Core;
using SimpleMessenger.Core.Model;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;

sealed class CreateNewChatMessageHandler : ServerMessageHandler<CreateNewChatMessage>
{
    protected override IResponse Process(CreateNewChatMessage message, ClientHandler client)
    {
        var target = FindUser(user => user.UID == message.Target, client.CurrentUser, client.Storage);
        if (target == null) return Error(ErrorMessage.UserNotFound);

        var sender = client.CurrentUser;
        var chat = sender.Chats.FirstOrDefault(c => c.Members.Contains(target));

        if(chat == null)
        {
            chat = new Chat { Members = new User[] { target, sender } };
            client.Storage.Chats.Add(chat);
        }

        var msg = new Message
        {
            Sender = sender,
            Content = message.Message,
            Time = DateTime.Now,
        };

        chat.AddMessage(msg);
        client.Storage.SaveChanges();


        //TODO: временно для тестов
        if (Server.TrySetHandler(target))
            target.Handler.SendAsync(new TextMessage(chat.Id, msg));

        return Json(chat.Id);
    }
}
