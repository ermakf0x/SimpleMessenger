using SimpleMessenger.Core.Messages;
using System.Text;

namespace SimpleMessenger.Core;

public class MessageSerializer : IMessageSerializer
{
    readonly Encoding _encoding;

    static readonly Dictionary<MessageType, Func<IMessage>> table = new()
    {
        { MessageType.HelloServer, () => new HelloServerMessage() },
        { MessageType.Registration, () => new RegistrationMessage() },
        { MessageType.Authorization, () => new AuthorizationMessage() },
        { MessageType.Success, () => new SuccessMessage() },
        { MessageType.Json, () => new JsonMessage() },
        { MessageType.Error, () => new ErrorMessage() },
        { MessageType.Text, () => new TextMessage() },
        { MessageType.FindUser, () => new FindUserMessage() },
    };

    public MessageSerializer(Encoding encoding)
    {
        _encoding = encoding;
    }

    public void Serialize(Stream stream, IMessage message)
    {
        stream.Write(message.MessageType);
        message.Write(new DataWriter(stream, _encoding));
    }

    public IMessage Desirialize(Stream stream)
    {
        var type = stream.Read<MessageType>();
        var message = table[type].Invoke();
        message.Read(new DataReader(stream, _encoding));
        return message;
    }
}
