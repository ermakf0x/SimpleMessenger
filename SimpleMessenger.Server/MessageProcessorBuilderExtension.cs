using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server;

static class MessageProcessorBuilderExtension
{
    public static MessageProcessorBuilder<ServerMessage> Bind2<T>(
        this MessageProcessorBuilder<ServerMessage> builder,
        IMessageHandler<ServerMessage> handler)
        where T : IMessage
    {
        return builder.Bind<T>(new ServerMessageHandler(handler));
    }

    class ServerMessageHandler : IMessageHandler<ServerMessage>
    {
        readonly IMessageHandler<ServerMessage> handler;

        public ServerMessageHandler(IMessageHandler<ServerMessage> handler)
        {
            this.handler = handler;
        }

        public void Process(ServerMessage message)
        {
            if (message.MSG is MessageBase msg)
            {
                if (msg.Token == Guid.Empty)
                {
                    message.MessageSerializer.Serialize(message.Stream, ErrorMsgHelper.NotAuthorized);
                    return;
                }
                handler.Process(message);
                return;
            }
        }
    }
}
