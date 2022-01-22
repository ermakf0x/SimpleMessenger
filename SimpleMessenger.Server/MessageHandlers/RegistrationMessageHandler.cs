using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;
class RegistrationMessageHandler : ServerMessageHandlerBase<RegistrationMessage>
{
    protected override IResponse Process(RegistrationMessage message, ClientHandler client)
    {
        try
        {
            var newUser = LocalDb.New(message.Login, message.Password, message.Name);
            client.CurrentUser = newUser;
            return Json(newUser.GetMainUser());
        }
        catch (ServerException ex)
        {
            return Error(ex);
        }
    }
}
