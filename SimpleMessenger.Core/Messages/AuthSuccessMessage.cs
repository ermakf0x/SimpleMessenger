using System;
using System.IO;

namespace SimpleMessenger.Core.Messages;

public sealed class AuthSuccessMessage : IMessage
{
    public MessageType Type => MessageType.AuthSuccess;
    public Guid Token { get; private set; }
    public int ID { get; private set; }

    public AuthSuccessMessage() { }
    public AuthSuccessMessage(Guid token, int id)
    {
        Token = token;
        ID = id;
    }

    public void Read(Stream stream)
    {
        Span<byte> buffer = new byte[32];
        var count = stream.Read(buffer);
        buffer = buffer[..count];
        Token = new Guid(buffer);
    }

    public void Write(Stream stream)
    {
        stream.Write(Token.ToByteArray());
    }

    public override string ToString() => Token.ToString();
}
