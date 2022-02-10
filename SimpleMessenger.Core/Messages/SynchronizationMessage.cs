using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Core.Messages;

public class SynchronizationMessage : BaseMessage
{
    public override MessageType MessageType => MessageType.Synchronization;
    public int UserHash { get; private set; }
    public int ChatHash { get; private set; }

    internal SynchronizationMessage() { }
    public SynchronizationMessage(Token token, ICollection<User> contacts, ICollection<Chat> chats) : base(token)
    {
        UserHash = HashCodeCombiner.Combine(contacts);
        ChatHash = HashCodeCombiner.Combine(chats);
    }

    protected override void Read(DataReader reader)
    {
        UserHash = reader.Read<int>();
        ChatHash = reader.Read<int>();
    }
    protected override void Write(DataWriter writer)
    {
        writer.Write(UserHash);
        writer.Write(ChatHash);
    }

    public override string ToString() => $"UserHash: { UserHash}; ChatHash: {ChatHash}";
}

public static class Synchronization
{
    [Flags]
    public enum State : byte
    {
        SyncSuccess         = 0,
        MustSyncContacts    = 1,
        MustSyncChats       = 2,
        MustSyncAll         = MustSyncContacts | MustSyncChats,
    }

    public enum Operation : byte
    {
        Add,
        Remove,
        Update
    }
}