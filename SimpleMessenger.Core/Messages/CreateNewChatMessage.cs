using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Core.Messages;

public class CreateNewChatMessage : BaseMessage
{
    public override MessageType MessageType => MessageType.CreateNewChat;
    public string Message { get; private set; }
    public int Target { get; private set; }

    internal CreateNewChatMessage() { }
    public CreateNewChatMessage(Token token, int target, string helloMessage) : base(token)
    {
        // TODO: нужна проверка входных параметров
        Message = helloMessage;
        Target = target;
    }

    protected override void Read(DataReader reader)
    {
        Target = reader.Read<int>();
        Message = reader.ReadString();
    }
    protected override void Write(DataWriter writer)
    {
        writer.Write(Target);
        writer.Write(Message ?? string.Empty);
    }

    public override string ToString() => $"Targer: {Target}; Message: '{Message}'";
}
