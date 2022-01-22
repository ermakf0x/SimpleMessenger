using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Core.Messages;

public class TextMessage : ChatableMessage
{
    public override MessageType MessageType { get; } = MessageType.Text;
    public string Message { get; set; }

    internal TextMessage() { }
    public TextMessage(Token token, Guid chatHash, int target, string message) : base(token, chatHash, target)
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

    public override string ToString() => Message;
}