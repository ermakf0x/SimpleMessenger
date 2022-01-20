using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Server.MessageHandlers;

class AuthorizationMessageHandler : ServerMessageHandlerBase<AuthorizationMessage>
{
    protected override IResponse Process(AuthorizationMessage message, ServerClient client)
    {
        var user = LocalDb.GetByLogin(message.Login);
        if (user == null) return Error(ErrorMessage.UserNotFound);
        if (user.Password != message.Password) return Error(ErrorMessage.PasswordInvalid);

        user.CurrentToken = Token.New();
        client.User = user;
        LocalDb.Update(user);

        return Json(client.User.CurrentToken);
    }
}
