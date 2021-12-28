using SimpleMessenger.Core.Messages;
using System;
using System.Collections.Generic;
using System.IO;

namespace SimpleMessenger.Core;

public class MessageSerializer : IMessageSerializer
{
    static readonly Dictionary<MessageType, Type> table = new()
    {
        { MessageType.Authorization, typeof(AuthorizationMessage) },
        { MessageType.AuthSuccess, typeof(AuthSuccessMessage) },
        { MessageType.Text, typeof(TextMessage) },
        { MessageType.Error, typeof(ErrorMessage) },
    };

    public void Serialize(Stream stream, IMessage message)
    {
        stream.Write(BitConverter.GetBytes((int)message.Type));
        message.Write(stream);
    }

    public IMessage Desirialize(Stream stream)
    {
        var buf = new byte[4];
        stream.Read(buf);
        var type = (MessageType)BitConverter.ToInt32(buf);
        var message = CreateCommandFromType(type);
        message.Read(stream);
        return message;
    }

    static IMessage CreateCommandFromType(MessageType type)
    {
        return (IMessage)Activator.CreateInstance(table[type]);
    }
}
