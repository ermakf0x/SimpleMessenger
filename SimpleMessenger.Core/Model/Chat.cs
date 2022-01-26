namespace SimpleMessenger.Core.Model;

public sealed class Chat
{
    public Guid ChatID { get; }
    public ICollection<Message> Messages { get; set; }
    public Message? LastMessage => Messages.LastOrDefault();

    public Chat(Guid chatID) : this(chatID, new List<Message>()) { }
    public Chat(Guid chatID, ICollection<Message> messages)
    {
        ChatID = chatID;
        Messages = messages;
    }
}