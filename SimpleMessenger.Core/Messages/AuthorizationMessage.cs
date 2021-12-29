using System.IO;
using System.Text;

namespace SimpleMessenger.Core.Messages;

public class AuthorizationMessage : IMessage
{
    public MessageType Type => MessageType.Authorization;
    public string Name { get; set; }

    public AuthorizationMessage() { }

    public void Read(Stream stream)
    {
        var reader = new StreamReader(stream, Encoding.ASCII);
        Name = reader.ReadToEnd();
    }

    public void Write(Stream stream)
    {
        stream.Write(Encoding.ASCII.GetBytes(Name));
    }
}
