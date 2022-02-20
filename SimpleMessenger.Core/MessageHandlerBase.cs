using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Core;

public abstract class MessageHandlerBase<TMsg> : IMessageHandler
    where TMsg : IMessage
{
    void IMessageHandler.Process(IMessage message, object? state) => Process((TMsg)message, state);
    protected abstract void Process(TMsg message, object? state);
}

public abstract class MessageHandlerBase<TMsg, TState> : IMessageHandler
    where TMsg : IMessage
    where TState : class
{
    void IMessageHandler.Process(IMessage message, object? state)
    {
        Process((TMsg)message, state as TState);
    }
    protected abstract void Process(TMsg message, TState? state);
}