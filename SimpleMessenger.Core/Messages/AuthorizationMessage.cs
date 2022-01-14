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

    void IMessage.Write(DataWriter writer)
    {
        writer.Write(Login);
        writer.Write(Password);
    }

    void IMessage.Read(DataReader reader)
    {
        Login = reader.ReadString();
        Password = reader.ReadString();
    }

    public override string ToString() => $"{Login}:{Password}";
}
