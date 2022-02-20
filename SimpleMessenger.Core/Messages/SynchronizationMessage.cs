using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Core.Messages;

public class SynchronizationMessage : BaseMessage
{
    public override MessageType MessageType => MessageType.Synchronization;
    public int UserHash { get; private set; }
    public int ChatHash { get; private set; }

    internal SynchronizationMessage() { }
    public SynchronizationMessage(Token token, IEnumerable<User> contacts, IEnumerable<Chat> chats) : base(token)
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