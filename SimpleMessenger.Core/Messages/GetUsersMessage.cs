using System.IO;

namespace SimpleMessenger.Core.Messages;

public class GetUsersMessage : IMessage
{
    public MessageType Type => MessageType.GetUsers;

    public void Read(Stream stream) { }
    public void Write(Stream stream) { }
}
