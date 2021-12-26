namespace SimpleMessenger.Core;

public interface IMessageHandler<TMsg> where TMsg : Message
{
    public void Process(TMsg message);
}
