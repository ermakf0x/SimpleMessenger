using System.IO;

namespace SimpleMessenger.Core;

public interface IMessage
{
    public MessageType MessageType { get; }
    public void Write(Stream stream);
    public void Read(Stream stream);
}

public enum MessageType : int
{
    Authorization,
    Error,
    Success,
    JsonContent,
    GetUsers,
    ResponseUsers,
    Text
}