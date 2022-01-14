namespace SimpleMessenger.Core.Messages;

public abstract class EmptyMessage : IMessage
{
    public abstract MessageType MessageType { get; }
    void IMessage.Write(DataWriter writer) { }
    void IMessage.Read(DataReader reader) { }

    public override string ToString()
    {
        return MessageType.ToString();
    }
}
