using System;
using System.IO;
using System.Text;

namespace SimpleMessenger.Core.Messages;

public class TextMessage : MessageBase
{
    public override MessageType Type { get; } = MessageType.Text;
    public string Text { get; protected set; }

    public TextMessage() { }
    public TextMessage(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentException($"\"{nameof(text)}\" не может быть неопределенным или пустым.", nameof(text));
        }

        Text = text;
    }

    public override void Write(Stream stream)
    {
        base.Write(stream);
        stream.Write(Encoding.ASCII.GetBytes(Text));
    }
    public override void Read(Stream stream)
    {
        base.Read(stream);
        var reader = new StreamReader(stream, Encoding.ASCII);
        Text = reader.ReadToEnd();
    }

    public override string ToString() => Text;
}