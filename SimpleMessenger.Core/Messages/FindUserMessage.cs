using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Core.Messages;

public class FindUserMessage : BaseMessage
{
    public override MessageType MessageType => MessageType.FindUser;
    public string Username { get; private set; }

    internal FindUserMessage() { }
    public FindUserMessage(string username, Token token) : base(token)
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new ArgumentException($"\"{nameof(username)}\" не может быть неопределенным или пустым.", nameof(username));
        }

        Username = username;
    }

    protected override void Read(DataReader reader)
    {
        Username = reader.ReadString();
    }
    protected override void Write(DataWriter writer)
    {
        writer.Write(Username);
    }

    public override string ToString() => $"Username: {Username}";
}