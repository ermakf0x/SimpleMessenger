namespace SimpleMessenger.Core.Model;

public sealed class Chat
{
    readonly List<Message> _messages = new();

    public int Id { get; init; }
    public IReadOnlyList<User> Members { get; init; } = new List<User>(2);
    public IReadOnlyCollection<Message> Messages => _messages;

    public void AddMessage(Message message)
    {
        _messages.Add(message);
    }
    public override string ToString() => $"{Members[0]?.Name} - {Members[1]?.Name}, Messages: {Messages.Count}";
}