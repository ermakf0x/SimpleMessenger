using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Core.Messages;

public record struct ChatSyncData(int ChatID, int HashSum);

public class SynchronizationChatsMessage : BaseMessage
{
    public override MessageType MessageType => MessageType.SynchronizationChats;
    public ChatSyncData[] SyncData { get; private set; }

    internal SynchronizationChatsMessage() { }
    public SynchronizationChatsMessage(Token token, IEnumerable<Chat> chats) : base(token)
    {
        SyncData = chats.Select(chat =>
        new ChatSyncData
        {
            ChatID = chat.Id,
            HashSum = chat.Hash
        }).ToArray();
    }

    protected override void Read(DataReader reader)
    {
        SyncData = reader.ReadArray<ChatSyncData>();
    }
    protected override void Write(DataWriter writer)
    {
        writer.Write(SyncData);
    }
}

public class SynchronizationChatMessage : BaseMessage
{
    public override MessageType MessageType => MessageType.SynchronizationChat;
    public int ChatID { get; private set; }
    public int[] ChunksHashSum { get; private set; }

    internal SynchronizationChatMessage() { }
    public SynchronizationChatMessage(Token token, Chat chat) : base(token)
    {
        ChatID = chat.Id;
        ChunksHashSum = chat.Chunks
            .Select(chunk => chunk.Hash)
            .ToArray();
    }

    protected override void Read(DataReader reader)
    {
        ChatID = reader.Read<int>();
        ChunksHashSum = reader.ReadArray<int>();
    }
    protected override void Write(DataWriter writer)
    {
        writer.Write(ChatID);
        writer.Write(ChunksHashSum);
    }
}