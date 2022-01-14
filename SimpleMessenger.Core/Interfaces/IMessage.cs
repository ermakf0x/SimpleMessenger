namespace SimpleMessenger.Core;

public interface IMessage
{
    MessageType MessageType { get; }
    void Write(DataWriter writer);
    void Read(DataReader reader);
}

public enum MessageType : int
{
    Registration,
    Authorization,
    Error,
    Success,
    Json,
    FindUser,
    Text
}