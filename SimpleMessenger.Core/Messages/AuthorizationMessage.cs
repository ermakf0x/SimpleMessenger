namespace SimpleMessenger.Core.Messages;

public sealed class AuthorizationMessage : IMessage
{
    public MessageType MessageType => MessageType.Authorization;
    public string Login { get; private set; }
    public string Password { get; private set; }

    internal AuthorizationMessage() { }
    public AuthorizationMessage(string login, string password)
    {
        Login = login ?? throw new ArgumentNullException(nameof(login));
        Password = password ?? throw new ArgumentNullException(nameof(password));
    }

    public void Read(Stream stream)
    {
        Login = stream.ReadString();
        Password = stream.ReadString();
    }

    public void Write(Stream stream)
    {
        stream.Write(Login);
        stream.Write(Password);
    }

    public override string ToString()
    {
        return $"{Login}:{Password}";
    }
}
