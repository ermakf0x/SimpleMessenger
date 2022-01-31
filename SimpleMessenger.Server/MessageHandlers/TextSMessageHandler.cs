using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Server.MessageHandlers;

class TextSMessageHandler : ServerMessageHandler<TextSMessage>
{
    protected override IResponse Process(TextSMessage message, ClientHandler client)
    {
        var target = FindUser(user => user.UID == message.Target, client.CurrentUser, client.Storage);
        if (target == null) return Error(ErrorMessage.UserNotFound);

        var sender = client.CurrentUser;
        var chat = sender.Chats.FirstOrDefault(c => c.Members.Contains(target));

        if(chat != null)
        {
            var msg = new Message
            {
                Sender = sender,
                Content = message.Message,
                Time = DateTime.Now,
            };
            chat.AddMessage(msg);
            client.Storage.Chats.Update(chat);
            client.Storage.SaveChanges();

            // TODO: временно для тестов
            if (Server.TrySetHandler(target))
                target.Handler.SendAsync(new TextMessage(message.ChatId, msg));

            return Json(msg.Id);
        }

        return Error(ErrorMessage.ChatNotFound);
    }
}
