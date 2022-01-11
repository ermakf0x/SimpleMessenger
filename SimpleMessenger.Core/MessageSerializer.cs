using SimpleMessenger.Core.Messages;
using System;
using System.Collections.Generic;
using System.IO;

namespace SimpleMessenger.Core;

public class MessageSerializer : IMessageSerializer
{
    static readonly Dictionary<MessageType, Func<IMessage>> table = new()
    {
        { MessageType.Authorization, () => new AuthorizationMessage() },
        { MessageType.Text, () => new TextMessage() },
        { MessageType.Success, () => new Success() },
        { MessageType.JsonContent, () => new JsonContent() },
        { MessageType.Error, () => new Error() },
        { MessageType.GetUsers, () => new GetUsersMessage() },
    };

    public void Serialize(Stream stream, IMessage message)
    {
        stream.Write(message.MessageType);
        message.Write(stream);
    }

    public IMessage Desirialize(Stream stream)
    {
        var type = stream.Read<MessageType>();
        var message = table[type].Invoke();
        message.Read(stream);
        return message;
    }
}
