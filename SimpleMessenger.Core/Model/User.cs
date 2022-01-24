namespace SimpleMessenger.Core.Model;

public class User
{
    public int UID { get; set; }
    public string Name { get; set; }
    public override string ToString() => Name ?? "";
}