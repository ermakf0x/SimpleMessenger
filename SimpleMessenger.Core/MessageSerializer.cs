using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Core;

public class MessageSerializer : IMessageSerializer
{
    static readonly Dictionary<MessageType, Func<IMessage>> table = new()
    {
        { MessageType.Registration, () => new RegistrationMessage() },
        { MessageType.Authorization, () => new AuthorizationMessage() },
        { MessageType.Success, () => new SuccessMessage() },
        { MessageType.Json, () => new JsonMessage() },
        { MessageType.Error, () => new ErrorMessage() },
        { MessageType.Text, () => new TextMessage() },
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
