using Microsoft.EntityFrameworkCore;
using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;
using SimpleMessenger.Server.Model;

namespace SimpleMessenger.Server.MessageHandlers;
class RegistrationMessageHandler : ServerMessageSlimHandler<RegistrationMessage>
{
    protected override IResponse Process(RegistrationMessage message, ClientHandler client)
    {
        var user = client.Storage.Users.Where(u => u.Username == message.Username)
                                       .Include(u => u.Chats)
                                       .Include(u => u.Contacts)
                                       .FirstOrDefault();
        if (user is not null)
            return Error(ErrorMessage.UsernameIsTaken);

        user = new User2
        {
            Username = message.Username,
            Password = message.Password,
            Name = message.Name,
            Token = Token.New(),
            RegDate = DateTime.Now,
        };
        client.Storage.Users.Add(user);
        client.Storage.SaveChanges();

        client.CurrentUser = user;
        return Json(user.GetAsMainUser());
    }
}
