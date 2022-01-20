using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Core.Messages;

public class FindUserMessage : MessageBase
{
    public override MessageType MessageType => MessageType.FindUser;
    public string UserName { get; private set; }

    internal FindUserMessage() { }
    public FindUserMessage(string username, Token token) : base(token)
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new ArgumentException($"\"{nameof(username)}\" не может быть неопределенным или пустым.", nameof(username));
        }

        UserName = username;
    }

    protected override void Read(DataReader reader)
    {
        UserName = reader.ReadString();
    }
    protected override void Write(DataWriter writer)
    {
        writer.Write(UserName);
    }

    public override string ToString() => UserName;
}
