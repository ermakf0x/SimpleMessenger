namespace SimpleMessenger.Core;

public interface IMessage
{
    MessageType MessageType { get; }
    void Write(DataWriter writer);
    void Read(DataReader reader);
}

public enum MessageType : int
{
    HelloServer,
    Registration,
    Authorization,
    Error,
    Success,
    Json,
    FindUser,
    RequestToMessaging,
    CreateNewChat,
    Text
}