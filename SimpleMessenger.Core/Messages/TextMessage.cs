using System;
using System.IO;
using System.Text;

namespace SimpleMessenger.Core;

public class TextMessage : IMessage
{
    public MessageType Type { get; } = MessageType.Text;
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

    public void Write(Stream stream)
    {
        stream.Write(Encoding.ASCII.GetBytes(Text));
    }
    public void Read(Stream stream)
    {
        var reader = new StreamReader(stream, Encoding.ASCII);
        Text = reader.ReadToEnd();
    }

    public override string ToString() => Text;
}