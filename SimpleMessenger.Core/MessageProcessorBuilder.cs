using System;
using System.Collections.Generic;

namespace SimpleMessenger.Core;

public sealed class MessageProcessorBuilder
{
    readonly Dictionary<Type, IMessageHandler> _handlers = new();

    public MessageProcessorBuilder Bind<T>(IMessageHandler handler) where T : IMessage
    {
        var msgType = typeof(T);
        if (!_handlers.ContainsKey(msgType))
        {
            _handlers.Add(msgType, handler);
        }

        return this;
    }
    public MessageProcessorBuilder Bind<T>(Action<T> action) where T : IMessage => Bind<T>(new DelegateMessageHandler<T>(action));

    public IMessageProcessor Build() => new MessageProcessor(_handlers);

    class MessageProcessor : IMessageProcessor
    {
        readonly Dictionary<Type, IMessageHandler> _handlers;

        public MessageProcessor(Dictionary<Type, IMessageHandler> handlers)
        {
            _handlers = handlers;
        }

        public void Push(IMessage message)
        {
            if (message == null) return;

            if (_handlers.TryGetValue(message.GetType(), out var handler))
            {
                handler.Process(message);
            }
        }
    }

    class DelegateMessageHandler<T> : IMessageHandler where T : IMessage
    {
        readonly Action<T> action;

        public DelegateMessageHandler(Action<T> action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public void Process(IMessage message)
        {
            action.Invoke((T)message);
        }
    }
}
