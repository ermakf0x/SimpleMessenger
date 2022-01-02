using System;
using System.Collections.Generic;

namespace SimpleMessenger.Core;

public sealed class MessageProcessorBuilder
{
    readonly Dictionary<Type, IMessageHandler> _handlers = new();

    public MessageProcessorBuilder Bind<T>(IMessageHandler handler)
        where T : IMessage
    {
        var msgType = typeof(T);
        if (!_handlers.ContainsKey(msgType))
        {
            _handlers.Add(msgType, handler);
        }

        return this;
    }
    public MessageProcessorBuilder Bind<T, T2>()
        where T : IMessage
        where T2 : IMessageHandler, new()
    {
        return Bind<T>(new T2());
    }
    public MessageProcessorBuilder Bind<T>(Action<T> action)
        where T : IMessage
    {
        return Bind<T>(new DelegateHandler<T>(action));
    }

    public IMessageProcessor Build() => new MessageProcessor(_handlers);

    class MessageProcessor : IMessageProcessor
    {
        readonly Dictionary<Type, IMessageHandler> _handlers;
        public MessageProcessor(Dictionary<Type, IMessageHandler> handlers) => _handlers = handlers;

        public void Push(IMessage message, object state = null)
        {
            if (message == null) return;

            if (_handlers.TryGetValue(message.GetType(), out var handler))
            {
                handler.Process(message);
            }
        }
    }
    class DelegateHandler<T> : IMessageHandler
        where T : IMessage
    {
        readonly Action<T> action;

        public DelegateHandler(Action<T> action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public void Process(IMessage message, object state = null)
        {
            action.Invoke((T)message);
        }
    }
}
