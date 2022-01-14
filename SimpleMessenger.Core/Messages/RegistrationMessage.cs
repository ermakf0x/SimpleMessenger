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

    void IMessage.Write(DataWriter writer)
    {
        writer.Write(Login);
        writer.Write(Password);
        writer.Write(Name);
    }
    void IMessage.Read(DataReader reader)
    {
        Login = reader.ReadString();
        Password = reader.ReadString();
        Name = reader.ReadString();
    }

    public override string ToString() => $"Login: {Login}; Password: {Password}; Name: {Name}";
}