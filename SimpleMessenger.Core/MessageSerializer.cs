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
        { MessageType.AuthSuccess, () => new AuthSuccessMessage() },
        { MessageType.Text, () => new TextMessage() },
        { MessageType.Error, () => new ErrorMessage() },
        { MessageType.GetUsers, () => new GetUsersMessage() },
        { MessageType.ResponseUsers, () => new ResponseUsersMessage() },
    };

    public void Serialize(Stream stream, IMessage message)
    {
        stream.Write(message.Type);
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
