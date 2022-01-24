namespace SimpleMessenger.Core.Model;

public sealed class Chat
{
    readonly List<Message> _messages = new();

    public Guid ChatID { get; }
    public IReadOnlyCollection<Message> Messages => _messages;
    public Message? LastMessage => Messages.LastOrDefault();

    public Chat(Guid chatID)
    {
        ChatID = chatID;
    }

    public void AddMessage(Message message)
    {
        ArgumentNullException.ThrowIfNull(message);
        _messages.Add(message);
    }
}