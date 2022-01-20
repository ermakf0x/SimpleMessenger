using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Core.Messages;

public class TextMessage : MessageBase
{
    public override MessageType MessageType { get; } = MessageType.Text;
    public string Text { get; set; }

    internal TextMessage() { }
    public TextMessage(Token token, string text) : base(token)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentException($"\"{nameof(text)}\" не может быть неопределенным или пустым.", nameof(text));
        }

        Text = text;
    }

    protected override void Write(DataWriter writer)
    {
        writer.Write(Text);
    }
    protected override void Read(DataReader reader)
    {
        Text = reader.ReadString();
    }

    public override string ToString() => Text;
}