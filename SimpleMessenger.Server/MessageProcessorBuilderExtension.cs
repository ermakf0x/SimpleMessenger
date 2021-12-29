using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server;

static class MessageProcessorBuilderExtension
{
    public static MessageProcessorBuilder<ServerClient> Bind2<T>(
        this MessageProcessorBuilder<ServerClient> builder,
        IMessageHandler<ServerClient> handler)
        where T : IMessage
    {
        return builder.Bind<T>(new ServerMessageHandler(handler));
    }
    public static MessageProcessorBuilder<ServerClient> Bind2<T>(this MessageProcessorBuilder<ServerClient> builder, Action<T> action)
        where T : IMessage
    {
        return builder.Bind<T>(new ServerMessageHandler(new DelegateMessageHandler<T>(action)));
    }

    class DelegateMessageHandler<T> : IMessageHandler<ServerClient> where T : IMessage
    {
        readonly Action<T> action;

        public DelegateMessageHandler(Action<T> action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public void Process(ServerClient message)
        {
            action.Invoke((T)message.MSG);
        }
    }

    class ServerMessageHandler : IMessageHandler<ServerClient>
    {
        readonly IMessageHandler<ServerClient> handler;

        public ServerMessageHandler(IMessageHandler<ServerClient> handler)
        {
            this.handler = handler;
        }

        public void Process(ServerClient message)
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
