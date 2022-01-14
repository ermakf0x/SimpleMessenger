namespace SimpleMessenger.Core.Messages;

public class FindUserMessage : Message
{
    public override MessageType MessageType => MessageType.FindUser;
    public string UserName { get; private set; }

    internal FindUserMessage() { }
    public FindUserMessage(string userName, Token token) : base(token)
    {
        if (string.IsNullOrEmpty(userName))
        {
            throw new ArgumentException($"\"{nameof(userName)}\" не может быть неопределенным или пустым.", nameof(userName));
        }

        UserName = userName;
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
