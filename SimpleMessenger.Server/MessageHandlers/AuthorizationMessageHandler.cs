using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Server.MessageHandlers;

class AuthorizationMessageHandler : ServerMessageSlimHandler<AuthorizationMessage>
{
    protected override IResponse Process(AuthorizationMessage message, ClientHandler client)
    {
        var user = LocalDb.GetUserByUsername(message.Username);
        if (user == null) return Error(ErrorMessage.UserNotFound);
        if (user.Password != message.Password) return Error(ErrorMessage.PasswordInvalid);

        user.Token = Token.New();
        client.CurrentUser = user;
        LocalDb.Update(user);

        return Json(client.CurrentUser.GetAsMainUser());
    }
}
