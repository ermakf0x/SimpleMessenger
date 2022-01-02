using System;
using System.Collections.Generic;

namespace SimpleMessenger.Core;

public sealed class MessageProcessorBuilder
{
    readonly Dictionary<Type, IMessageHandler> _handlers = new();
    Action<IMessage> _default;
    Action<IMessage, Exception> _onError;

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

    public MessageProcessorBuilder Default(Action<IMessage> handler)
    {
        _default = handler;
        return this;
    }
    public MessageProcessorBuilder OnError(Action<IMessage, Exception> handler)
    {
        _onError = handler;
        return this;
    }

    public IMessageProcessor Build() => new MessageProcessor(_handlers, _default, _onError ?? OnError);

    void OnError(IMessage message, Exception ex)
    {
        Console.WriteLine($"Error: {ex}\r\nMessage: {message}");
    }

    class MessageProcessor : IMessageProcessor
    {
        readonly Dictionary<Type, IMessageHandler> _handlers;
        readonly Action<IMessage> _default;
        readonly Action<IMessage, Exception> _onError;

        public MessageProcessor(Dictionary<Type, IMessageHandler> handlers, Action<IMessage> @default, Action<IMessage, Exception> onError)
        {
            _handlers = handlers;
            _default = @default;
            _onError = onError;
        }

        public void Push(IMessage message, object state = null)
        {
            if (message == null) return;

            try
            {
                if (_handlers.TryGetValue(message.GetType(), out var handler))
                {
                    handler.Process(message, state);
                }
                else _default?.Invoke(message);
            }
            catch (Exception ex)
            {
                _onError.Invoke(message, ex);
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
