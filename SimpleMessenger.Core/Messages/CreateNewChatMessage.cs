using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Core.Messages;

public class CreateNewChatMessage : BaseMessage
{
    public override MessageType MessageType => MessageType.CreateNewChat;
    public string HelloMessage { get; private set; }
    public int Target { get; private set; }

    internal CreateNewChatMessage() { }
    public CreateNewChatMessage(Token token, int target, string helloMessage) : base(token)
    {
        // TODO: нужна проверка входных параметров
        HelloMessage = helloMessage;
        Target = target;
    }

    protected override void Read(DataReader reader)
    {
        Target = reader.Read<int>();
        HelloMessage = reader.ReadString();
    }

    protected override void Write(DataWriter writer)
    {
        writer.Write(Target);
        writer.Write(HelloMessage ?? string.Empty);
    }
}
