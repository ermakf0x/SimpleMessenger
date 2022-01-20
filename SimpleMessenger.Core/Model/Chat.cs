namespace SimpleMessenger.Core.Model;

public sealed class Chat
{
    readonly List<Message> _messages = new();

    public Guid Hash { get; }
    public IReadOnlyCollection<Message> Messages => _messages;

    public Chat(Guid hash)
    {
        Hash = hash;
    }

    public void AddMessage(Message message)
    {
        ArgumentNullException.ThrowIfNull(message);
        _messages.Add(message);
    }

    public static Chat Create() => new(Guid.NewGuid());
}