namespace SimpleMessenger.Core.Model;

public class User
{
    public int Id { get; init; }
    public string Name { get; set; }
    public override string ToString() => Name ?? "";
}