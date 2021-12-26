using System;
using System.Collections.Generic;

namespace SimpleMessenger.Core;

public sealed class MessageProcessorBuilder<TMsg> where TMsg : Message
{
    readonly Dictionary<Type, IMessageHandler<TMsg>> _handlers = new();
    public static MessageProcessorBuilder<Message> Default => new();

    public MessageProcessorBuilder<TMsg> Bind<T>(IMessageHandler<TMsg> handler) where T : IMessage
    {
        var msgType = typeof(T);
        if (!_handlers.ContainsKey(msgType))
        {
            _handlers.Add(msgType, handler);
        }

        return this;
    }
    public MessageProcessorBuilder<TMsg> Bind<T>(Action<T> action) where T : IMessage
    {
        return Bind<T>(new DelegateMessageHandler<T>(action));
    }

    public IMessageProcessor<TMsg> Build() => new MessageProcessor(_handlers);

    class MessageProcessor : IMessageProcessor<TMsg>
    {
        readonly Dictionary<Type, IMessageHandler<TMsg>> _handlers;

        public MessageProcessor(Dictionary<Type, IMessageHandler<TMsg>> handlers)
        {
            _handlers = handlers;
        }

        public void Push(TMsg message)
        {
            if (message == null) return;

            if (_handlers.TryGetValue(message.MSG.GetType(), out var handler))
            {
                handler.Process(message);
            }
        }
    }
    class DelegateMessageHandler<T> : IMessageHandler<TMsg>where T : IMessage
    {
        readonly Action<T> action;

        public DelegateMessageHandler(Action<T> action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public void Process(TMsg message)
        {
            action.Invoke((T)message.MSG);
        }
    }
}
