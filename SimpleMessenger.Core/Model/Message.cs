namespace SimpleMessenger.Core.Model;

public class Message
{
    public User Owner { get; init; }
    public string Content { get; set; }

    public Message(User owner, string content)
    {
        Owner = owner ?? throw new ArgumentNullException(nameof(owner));
        Content = content ?? throw new ArgumentNullException(nameof(content));
    }

    public override bool Equals(object? obj) => Owner.Equals(obj);
    public override int GetHashCode() => Owner.GetHashCode();
    public override string ToString() => Content;
}