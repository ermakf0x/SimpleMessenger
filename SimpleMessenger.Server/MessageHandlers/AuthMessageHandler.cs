using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;

class AuthMessageHandler : ServerMessageHandlerBase
{
    protected override void Process(IMessage message, ServerClient client)
    {
        if (message is AuthorizationMessage authMsg)
        {
            client.User = LocalDB.New(authMsg.Name);
            client.SendAsync(new AuthSuccessMessage(client.User.Token, client.User.Data.Id));
        }
    }
}
