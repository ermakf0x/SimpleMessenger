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
    Synchronization,
    SynchronizationContacts,
    Error,
    Success,
    Json,
    FindUser,
    GetUser,
    CreateNewChat,
    TextS,
    Text
}