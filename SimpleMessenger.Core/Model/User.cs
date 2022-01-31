using System.ComponentModel.DataAnnotations;

namespace SimpleMessenger.Core.Model;

public class User
{
    [Key]
    public int UID { get; init; }
    public string Username { get; set; }
    public string Name { get; set; }
    public override string ToString() => $"UID = {UID}; Name = {Name ?? "user"}";
}

public class MainUser : User
{
    public Token Token { get; set; }
}