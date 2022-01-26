using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Server.MessageHandlers;

class HelloServerMessageHandler : ServerMessageSlimHandler<HelloServerMessage>
{
    protected override IResponse Process(HelloServerMessage message, ClientHandler client)
    {
        if(client.CurrentUser != null)
            return Success();

        if (message.Token == Token.Empty)
            return Error(ErrorMessage.TokenInvalid);

        var user = LocalDb.GetUserByToken(message.Token);
        if(user != null)
        {
            client.CurrentUser = user;
            return Success();
        }
        return Error(ErrorMessage.TokenInvalid);
    }
}
