using System;
using System.IO;

namespace SimpleMessenger.Core.Messages;

public sealed class RegistrationMessage : IMessage
{
    public MessageType MessageType => MessageType.Registration;
    public string Login { get; private set; }
    public string Password { get; private set; }
    public string Name { get; private set; }

    internal RegistrationMessage() { }
    public RegistrationMessage(string login, string password, string name)
    {
        // TODO: Нужна проверка валидности логина и пароля
        Login = login ?? throw new ArgumentNullException(nameof(login));
        Password = password ?? throw new ArgumentNullException(nameof(password));
        Name = name ?? login;
    }

    public void Read(Stream stream)
    {
        Login = stream.ReadString();
        Password = stream.ReadString();
        Name = stream.ReadString();
    }

    public void Write(Stream stream)
    {
        stream.Write(Login);
        stream.Write(Password);
        stream.Write(Name);
    }

    public override string ToString()
    {
        return $"Login: {Login}; Password: {Password}; Name: {Name}";
    }
}