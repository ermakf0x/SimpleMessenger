using System.ComponentModel.DataAnnotations;

namespace SimpleMessenger.Core.Model;

public class User : IEquatable<User>
{
    [Key]
    public int UID { get; init; }
    public string Username { get; set; }
    public string Name { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is User user)
            return Equals(user);
        return false;
    }
    public bool Equals(User? other) => other?.UID == UID;
    public override int GetHashCode() => UID.GetHashCode();
    public override string ToString() => $"UID = {UID}; Name = {Name ?? "user"}";

    public static bool operator ==(User a, User b) => a.Equals(b);
    public static bool operator !=(User a, User b) => a != b;
}

public class MainUser : User
{
    public Token Token { get; set; }
}