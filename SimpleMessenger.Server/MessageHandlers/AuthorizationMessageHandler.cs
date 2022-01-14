using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;

class AuthorizationMessageHandler : ServerMessageHandlerBase<AuthorizationMessage>
{
    protected override IResponse Process(AuthorizationMessage message, ServerClient client)
    {
        var user = LocalDb.GetByLogin(message.Login);
        if (user == null) return Error($"Пользаватель с логином '{message.Login}' не найден.");
        if (user.Password != message.Password) return Error("Неверный пароль.");

        user.CurrentToken = Token.New();
        client.User = user;
        LocalDb.Update(user);

        return Json(client.User.CurrentToken);
    }
}
