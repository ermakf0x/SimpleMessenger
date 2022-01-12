namespace SimpleMessenger.Core;

public abstract class MessageHandlerBase<TMsg> : IMessageHandler
    where TMsg : IMessage
{
    public void Process(IMessage message, object state = null) => Process((TMsg)message, state);
    public abstract void Process(TMsg message, object state = null);
}
