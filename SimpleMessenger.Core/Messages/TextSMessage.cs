using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Core.Messages;

public class TextSMessage : ChatableMessage
{
    public override MessageType MessageType { get; } = MessageType.TextS;
    public string Message { get; set; }

    internal TextSMessage() { }
    public TextSMessage(Token token, int chatId, int target, string message) : base(token, chatId, target)
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
    public int ChatId { get; private set; }
    public Message Message { get; private set; }
    public int Sender => Message.SenderId;

    internal TextMessage() { }
    public TextMessage(int chatId, Message message)
    {
        ChatId = chatId;
        Message = message;
    }

    void IMessage.Read(DataReader reader)
    {
        ChatId = reader.Read<int>();
        Message = new Message
        {
            Id = reader.Read<int>(),
            SenderId = reader.Read<int>(),
            Time = reader.Read<TimeOnly>(),
            Content = reader.ReadString(),
        };
    }
    void IMessage.Write(DataWriter writer)
    {
        writer.Write(ChatId);
        writer.Write(Message.Id);
        writer.Write(Message.SenderId);
        writer.Write(Message.Time);
        writer.Write(Message.Content);
    }

    public override string ToString() => $"ChatId: {ChatId}; Message: {Message}";
}
