using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;

class AuthMessageHandler : ServerMessageHandlerBase
{
    protected override IResponse Process(IMessage message, ServerClient client)
    {
        var authMsg = message as AuthorizationMessage;
        client.User = LocalDB.New(authMsg.Name);
        return Content(client.User.Token);
    }
}
