namespace SimpleMessenger.Core.Model;

public sealed class Message
{
    public int Id { get; set; }
    public DateTime Time { get; init; }
    public string Content { get; set; }

    public int SenderId { get; set; }
    public User Sender { get; set; }

    public override string ToString()
        => $"{Id}, {Sender}, {Content}";
}