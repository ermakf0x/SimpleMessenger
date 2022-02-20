using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Core;

public interface IMessageProcessor
{
    void Push(IMessage message, object? state = null);
}