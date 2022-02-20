namespace SimpleMessenger.Core.Messages;

public interface IMessage
{
    MessageType MessageType { get; }
    void Write(DataWriter writer);
    void Read(DataReader reader);
}