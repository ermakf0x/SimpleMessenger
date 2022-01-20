using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Server.MessageHandlers;

class HelloServerMessageHandler : ServerMessageHandlerBase<HelloServerMessage>
{
    protected override IResponse Process(HelloServerMessage message, ServerClient client)
    {
        if(client.User != null)
            return Success();

        if (message.Token == Token.Empty)
            return Error(ErrorMessage.TokenInvalid);

        var user = LocalDb.GetByToken(message.Token);
        if(user != null)
        {
            client.User = user;
            return Success();
        }
        return Error(ErrorMessage.TokenInvalid);
    }
}
