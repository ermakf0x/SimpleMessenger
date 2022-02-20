namespace SimpleMessenger.Core.Messages;

public enum MessageType : int
{
    HelloServer,
    Registration,
    Authorization,
    Synchronization,
    SynchronizationChats,
    SynchronizationChat,
    SynchronizationContacts,
    Error,
    Success,
    Json,
    FindUser,
    GetUser,
    GetChat,
    CreateNewChat,
    TextS,
    Text
}