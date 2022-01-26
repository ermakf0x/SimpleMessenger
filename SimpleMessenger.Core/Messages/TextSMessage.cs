using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Core.Messages;

public class TextSMessage : ChatableMessage
{
    public override MessageType MessageType { get; } = MessageType.TextS;
    public string Message { get; set; }

    internal TextSMessage() { }
    public TextSMessage(Token token, Guid chatHash, int target, string message) : base(token, chatHash, target)
    {
        if (string.IsNullOrEmpty(message))
        {
            throw new ArgumentException($"\"{nameof(message)}\" не может быть неопределенным или пустым.", nameof(message));
        }

        Message = message;
    }

    protected override void Write(DataWriter writer)
    {
        writer.Write(Message);
    }
    protected override void Read(DataReader reader)
    {
        Message = reader.ReadString();
    }

    public override string ToString() => $"{base.ToString()}; Message: {Message}";
}

public class TextMessage : IMessage
{
    public MessageType MessageType => MessageType.Text;
    public Guid ChatID { get; private set; }
    public int Sender { get; private set; }
    public string Message { get; private set; }

    internal TextMessage() { }
    public TextMessage(Guid chatID, int sender, string message)
    {
        ChatID = chatID;
        Sender = sender;
        Message = message;
    }

    void IMessage.Read(DataReader reader)
    {
        ChatID = reader.Read<Guid>();
        Sender = reader.Read<int>();
        Message = reader.ReadString();
    }
    void IMessage.Write(DataWriter writer)
    {
        writer.Write(ChatID);
        writer.Write(Sender);
        writer.Write(Message);
    }

    public override string ToString() => $"ChatID: {ChatID}; Sender: {Sender}; Message: {Message}";
}
