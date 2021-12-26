namespace SimpleMessenger.Core
{
    public interface IMessageProcessor<TMsg> where TMsg : Message
    {
        void Push(TMsg message);
    }
}