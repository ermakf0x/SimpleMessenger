using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;

class AuthorizationMessageHandler : ServerMessageHandlerBase<AuthorizationMessage>
{
    protected override IResponse Process(AuthorizationMessage message, ServerClient client)
    {
        client.User = LocalDb.New(message.Name);
        return Content(client.User.CurrentToken);
    }
}
