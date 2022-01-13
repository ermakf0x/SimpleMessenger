namespace SimpleMessenger.Core;

public interface IMessageHandler
{
    public void Process(IMessage message, object? state = null);
}
