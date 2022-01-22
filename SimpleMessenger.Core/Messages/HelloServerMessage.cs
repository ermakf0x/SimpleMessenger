using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Core.Messages;

public class HelloServerMessage : BaseMessage
{
    public override MessageType MessageType => MessageType.HelloServer;

    internal HelloServerMessage() { }
    public HelloServerMessage(Token token) : base(token) { }
    protected override void Read(DataReader reader) { }
    protected override void Write(DataWriter writer) { }
    public override string ToString() => Token.ToString();
}
