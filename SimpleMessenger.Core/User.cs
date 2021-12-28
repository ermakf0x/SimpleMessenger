using System;

namespace SimpleMessenger.Core;

public class User
{
    public UserData Data { get; set; }
    public Guid Token { get; set; }

    public User() { }
    public User(string name)
    {
        Data = new UserData { Name = name };
    }
    public override string ToString() => Data.ToString();
}

public class UserData
{
    public int Id { get; set; }
    public string Name { get; set; }

    public override string ToString() => $"ID: {Id} Name: {Name}";
}