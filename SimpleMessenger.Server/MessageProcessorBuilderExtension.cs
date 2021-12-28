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
    public static MessageProcessorBuilder<ServerMessage> Bind2<T>(this MessageProcessorBuilder<ServerMessage> builder, Action<T> action)
        where T : IMessage
    {
        return builder.Bind<T>(new ServerMessageHandler(new DelegateMessageHandler<T>(action)));
    }

    class DelegateMessageHandler<T> : IMessageHandler<ServerMessage> where T : IMessage
    {
        readonly Action<T> action;

        public DelegateMessageHandler(Action<T> action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public void Process(ServerMessage message)
        {
            action.Invoke((T)message.MSG);
        }
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
                    Helper.WriteMessageAsync(message.Stream, message.MessageSerializer, ErrorMsgHelper.NotAuthorized);
                    return;
                }
                handler.Process(message);
                return;
            }
        }
    }
}
