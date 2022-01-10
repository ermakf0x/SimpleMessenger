using System;
using System.IO;

namespace SimpleMessenger.Core.Messages;

public sealed class AuthSuccessMessage : IMessage
{
    public MessageType Type => MessageType.AuthSuccess;
    public Token Token { get; private set; }
    public int ID { get; private set; }

    internal AuthSuccessMessage() { }
    public AuthSuccessMessage(Token token, int id)
    {
        Token = token;
        ID = id;
    }

    public void Read(Stream stream)
    {
        Token = stream.Read<Token>();
        ID = stream.Read<int>();
    }

    public void Write(Stream stream)
    {
        stream.Write(Token);
        stream.Write(ID);
    }

    public override string ToString() => Token.ToString();
}
