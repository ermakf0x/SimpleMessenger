using SimpleMessenger.Core;
using SimpleMessenger.Core.Model;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;

sealed class CreateNewChatMessageHandler : ServerMessageHandler<CreateNewChatMessage>
{
    protected override IResponse Process(CreateNewChatMessage message, ClientHandler client)
    {
        var sender = client.CurrentUser;
        var target = FindUser(user => user.UID == message.Target, sender, client.Storage);
        if (target is null) return Error(ErrorMessage.UserNotFound);

        var chat = sender.Chats.FirstOrDefault(c => c.Members.Contains(target), new Chat{ Members = new User[] { target, sender } });
        var msg = new Message
        {
            Sender = sender,
            Content = message.Message,
            Time = TimeOnly.FromDateTime(DateTime.Now),
        };
        chat.AddMessage(msg);

        if(chat.Id == 0)
        {
            sender.Chats.Add(chat);
            target.Chats.Add(chat);
            client.Storage.Chats.Add(chat);
            client.Storage.Update(sender);
            client.Storage.Update(target);
        }
        else
        {
            client.Storage.Update(chat);
        }
        client.Storage.SaveChanges();


        //TODO: временно для тестов
        if (Server.TrySetHandler(target))
            target.Handler.SendAsync(new TextMessage(chat.Id, msg));

        return Json(chat.Id);
    }
}
