namespace SimpleMessenger.Core.Messages;

public abstract class EmptyMessage : IMessage
{
    public abstract MessageType MessageType { get; }

    void IMessage.Read(Stream stream) { }
    void IMessage.Write(Stream stream) { }

    public override string ToString()
    {
        return MessageType.ToString();
    }
}
