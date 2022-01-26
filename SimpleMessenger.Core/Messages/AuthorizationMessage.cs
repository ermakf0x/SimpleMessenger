namespace SimpleMessenger.Core.Messages;

public sealed class AuthorizationMessage : IMessage
{
    public MessageType MessageType => MessageType.Authorization;
    public string Username { get; private set; }
    public string Password { get; private set; }

    internal AuthorizationMessage() { }
    public AuthorizationMessage(string username, string password)
    {
        Username = username ?? throw new ArgumentNullException(nameof(username));
        Password = password ?? throw new ArgumentNullException(nameof(password));
    }

    void IMessage.Write(DataWriter writer)
    {
        writer.Write(Username);
        writer.Write(Password);
    }

    void IMessage.Read(DataReader reader)
    {
        Username = reader.ReadString();
        Password = reader.ReadString();
    }

    public override string ToString() => $"Username: {Username}; Password: {Password}";
}
