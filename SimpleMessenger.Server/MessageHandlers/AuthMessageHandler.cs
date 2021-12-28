using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;

class AuthMessageHandler : IMessageHandler<ServerMessage>
{
    public void Process(ServerMessage message)
    {
        Helper.WriteMessageAsync(message.Stream, message.MessageSerializer, new AuthSuccessMessage(Guid.NewGuid()));
    }
}
