namespace SimpleMessenger.Core
{
    public interface IMessageProcessor
    {
        void Push(IMessage message);
    }
}