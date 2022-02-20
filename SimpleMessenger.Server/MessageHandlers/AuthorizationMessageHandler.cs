using Microsoft.EntityFrameworkCore;
using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Server.MessageHandlers;

class AuthorizationMessageHandler : ServerMessageSlimHandler<AuthorizationMessage>
{
    protected override IResponse Process(AuthorizationMessage message, ClientHandler client)
    {
        var user = client.Storage.Users
            .Where(u => u.Username == message.Username)
            .FirstOrDefault();

        if (user is null)
            return Error(ErrorMessage.UserNotFound);
        if (user.Password != message.Password)
            return Error(ErrorMessage.PasswordInvalid);

        user = client.Storage.Users
            .Where(u => u.UID == user.UID)
            .Include(u => u.Chats.Where(c => c.FirstUserID == user.UID || c.SecondUserID == user.UID))
            .Include(u => u.Contacts)
            .First();

        user.Token = Token.New();
        client.Storage.Update(user);
        client.CurrentUser = user;
        client.Storage.SaveChanges();
        return Json(user.GetAsMainUser());
    }
}