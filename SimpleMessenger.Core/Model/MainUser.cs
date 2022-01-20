namespace SimpleMessenger.Core.Model;

public sealed class MainUser : User
{
    public Token Token { get; init; }
}