using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Core.Messages;

public abstract class ChatableMessage : ITokenable
{
    public abstract MessageType MessageType { get; }
    public Token Token { get; private set; }
    public int Target { get; private set; }
    public int ChatId { get; private set; }

    protected ChatableMessage() { }
    protected ChatableMessage(Token token, int chatId, int target)
    {
        Token = token;
        ChatId = chatId;
        Target = target;
    }

    void IMessage.Write(DataWriter writer)
    {
        writer.Write(Token);
        writer.Write(ChatId);
        writer.Write(Target);
        Write(writer);
    }
    void IMessage.Read(DataReader reader)
    {
        Token = reader.Read<Token>();
        ChatId = reader.Read<int>();
        Target = reader.Read<int>();
        Read(reader);
    }

    protected abstract void Write(DataWriter writer);
    protected abstract void Read(DataReader reader);
    public override string ToString() => $"Token: {Token}; ChatId: {ChatId}; Target: {Target}";
}
