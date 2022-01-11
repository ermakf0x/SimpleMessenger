using System;
using System.IO;

namespace SimpleMessenger.Core.Messages;

public class TextMessage : Message
{
    public override MessageType MessageType { get; } = MessageType.Text;
    public string Text { get; set; }

    internal TextMessage() { }
    public TextMessage(Token token, string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentException($"\"{nameof(text)}\" не может быть неопределенным или пустым.", nameof(text));
        }

        Text = text;
        Token = token;
    }

    public override void Write(Stream stream)
    {
        base.Write(stream);
        stream.Write(Text);
    }
    public override void Read(Stream stream)
    {
        base.Read(stream);
        Text = stream.ReadString();
    }

    public override string ToString() => Text;
}