using System;

namespace SimpleMessenger.Core;
public class Message
{
    public IMessage MSG { get; }

    public Message(IMessage message)
    {
        MSG = message ?? throw new ArgumentNullException(nameof(message));
    }
}
