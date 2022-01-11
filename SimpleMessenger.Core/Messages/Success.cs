using System.IO;

namespace SimpleMessenger.Core.Messages;

public class Success : IResponse
{
    public MessageType MessageType => MessageType.Success;
    public void Read(Stream stream) { }
    public void Write(Stream stream) { }
    public override string ToString() => "Success";
}
