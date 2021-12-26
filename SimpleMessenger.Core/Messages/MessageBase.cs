using System;
using System.IO;

namespace SimpleMessenger.Core.Messages;

public abstract class MessageBase : IMessage
{
    public abstract MessageType Type { get; }
    public Guid Token { get; private set; }

    public MessageBase(Guid token)
    {
        Token = token;
    }

    public virtual void Read(Stream stream)
    {
        var buf = new byte[16];
        stream.Read(buf, 0, buf.Length);
        Token = new Guid(buf);
    }

    public virtual void Write(Stream stream)
    {
        stream.Write(Token.ToByteArray());
    }
}
