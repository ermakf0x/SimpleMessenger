using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;
class RegistrationMessageHandler : ServerMessageSlimHandler<RegistrationMessage>
{
    protected override IResponse Process(RegistrationMessage message, ClientHandler client)
    {
        try
        {
            var newUser = LocalDb.CreateNewUser(message.Username, message.Password, message.Name);
            client.CurrentUser = newUser;
            return Json(newUser.GetAsMainUser());
        }
        catch (ServerException ex)
        {
            return Error(ex);
        }
    }
}
