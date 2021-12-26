using System.IO;

namespace SimpleMessenger.Core;

public interface IMessage
{
    public MessageType Type { get; }
    public void Write(Stream stream);
    public void Read(Stream stream);
}

public enum MessageType : int
{
    Authorization,
    Error,
    Text
}