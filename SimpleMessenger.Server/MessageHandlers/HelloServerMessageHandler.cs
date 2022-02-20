using Microsoft.EntityFrameworkCore;
using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Server.MessageHandlers;

class HelloServerMessageHandler : ServerMessageSlimHandler<HelloServerMessage>
{
    protected override IResponse Process(HelloServerMessage message, ClientHandler client)
    {
        if(client.CurrentUser is not null)
            return Success();

        if (message.Token == Token.Empty)
            return Error(ErrorMessage.TokenInvalid);

        var user = client.Storage.Users
            .Where(u => u.Token == message.Token)
            .FirstOrDefault();

        if (user is not null)
        {
            user = client.Storage.Users
                .Where(u => u.UID == user.UID)
                .Include(u => u.Chats.Where(c => c.FirstUserID == user.UID || c.SecondUserID == user.UID))
                .Include(u => u.Contacts)
                .First();

            client.CurrentUser = user;
            return Success();
        }
        return Error(ErrorMessage.TokenInvalid);
    }
}
