namespace SimpleMessenger.Core;

public interface IMessage
{
    MessageType MessageType { get; }
    void Write(Stream stream);
    void Read(Stream stream);
}

public enum MessageType : int
{
    Registration,
    Authorization,
    Error,
    Success,
    Json,
    GetUsers,
    Text
}