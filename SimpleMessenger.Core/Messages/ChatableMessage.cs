using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Core.Messages;

public abstract class ChatableMessage : ITokenable
{
    public abstract MessageType MessageType { get; }
    public Token Token { get; private set; }
    public int Target { get; private set; }
    public Guid ChatID { get; private set; }

    protected ChatableMessage() { }
    protected ChatableMessage(Token token, Guid chatID, int target)
    {
        Token = token;
        ChatID = chatID;
        Target = target;
    }

    void IMessage.Write(DataWriter writer)
    {
        writer.Write(Token);
        writer.Write(ChatID);
        writer.Write(Target);
        Write(writer);
    }
    void IMessage.Read(DataReader reader)
    {
        Token = reader.Read<Token>();
        ChatID = reader.Read<Guid>();
        Target = reader.Read<int>();
        Read(reader);
    }

    protected abstract void Write(DataWriter writer);
    protected abstract void Read(DataReader reader);
    public override string ToString() => $"Token: {Token}; ChatID: {ChatID}; Target: {Target}";
}
