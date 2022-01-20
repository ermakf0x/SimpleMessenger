namespace SimpleMessenger.Core.Model;

public class Message
{
    public int Id { get; init; }
    public string Content { get; set; }

    public override bool Equals(object? obj) => Id.Equals(obj);
    public override int GetHashCode() => Id.GetHashCode();
    public override string ToString() => Content;
}