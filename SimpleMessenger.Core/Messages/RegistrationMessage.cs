namespace SimpleMessenger.Core.Messages;

public sealed class RegistrationMessage : IMessage
{
    public MessageType MessageType => MessageType.Registration;
    public string Username { get; private set; }
    public string Password { get; private set; }
    public string Name { get; private set; }

    internal RegistrationMessage() { }
    public RegistrationMessage(string username, string password, string name)
    {
        // TODO: Нужна проверка валидности логина и пароля
        Username = username ?? throw new ArgumentNullException(nameof(username));
        Password = password ?? throw new ArgumentNullException(nameof(password));
        Name = name ?? username;
    }

    void IMessage.Write(DataWriter writer)
    {
        writer.Write(Username);
        writer.Write(Password);
        writer.Write(Name);
    }
    void IMessage.Read(DataReader reader)
    {
        Username = reader.ReadString();
        Password = reader.ReadString();
        Name = reader.ReadString();
    }

    public override string ToString() => $"Username: {Username}; Password: {Password}; Name: {Name}";
}