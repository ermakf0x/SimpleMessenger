using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;

class AuthMessageHandler : IMessageHandler<ServerClient>
{
    public void Process(ServerClient client)
    {
        if (client.MSG is AuthorizationMessage authMsg)
        {
            client.User = LocalDB.New(authMsg.Name);
            Helper.WriteMessageAsync(client.Stream, client.MessageSerializer, new AuthSuccessMessage(client.User.Token, client.User.Data.Id));
        }
    }
}
