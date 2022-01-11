using System.IO;

namespace SimpleMessenger.Core.Messages;

public abstract class Message : IMessage
{
    public abstract MessageType MessageType { get; }
    public Token Token { get; set; }

    public virtual void Read(Stream stream)
    {
        Token = stream.Read<Token>();
    }

    public virtual void Write(Stream stream)
    {
        stream.Write(Token);
    }
}
