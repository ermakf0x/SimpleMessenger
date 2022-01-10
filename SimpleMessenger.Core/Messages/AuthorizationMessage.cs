using System;
using System.IO;

namespace SimpleMessenger.Core.Messages;

public class AuthorizationMessage : IMessage
{
    public MessageType Type => MessageType.Authorization;
    public string Name { get; set; }

    internal AuthorizationMessage() { }

    public AuthorizationMessage(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public void Read(Stream stream)
    {
        Name = stream.ReadString();
    }

    public void Write(Stream stream)
    {
        stream.Write(Name);
    }
}
