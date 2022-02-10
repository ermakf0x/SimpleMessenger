using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Server.MessageHandlers;

class TextSMessageHandler : ServerMessageHandler<TextSMessage>
{
    protected override IResponse Process(TextSMessage message, ClientHandler client)
    {
        var sender = client.CurrentUser;
        var target = FindUser(user => user.UID == message.Target, sender, client.Storage);
        if (target is null) return Error(ErrorMessage.UserNotFound);

        var chat = sender.Chats.FirstOrDefault(c => c.Members.Contains(target));

        if(chat is not null)
        {
            var msg = new Message
            {
                Sender = sender,
                Content = message.Message,
                Time = TimeOnly.FromDateTime(DateTime.Now),
            };
            chat.AddMessage(msg);
            client.Storage.Update(chat);
            client.Storage.SaveChanges();

            // TODO: временно для тестов
            if (Server.TrySetHandler(target))
                target.Handler.SendAsync(new TextMessage(message.ChatId, msg));

            return Json(msg.Id);
        }

        return Error(ErrorMessage.ChatNotFound);
    }
}
