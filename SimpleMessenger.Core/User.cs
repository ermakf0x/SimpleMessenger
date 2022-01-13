namespace SimpleMessenger.Core;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Token Token { get; set; }
    public override string ToString() => Name ?? "";
}