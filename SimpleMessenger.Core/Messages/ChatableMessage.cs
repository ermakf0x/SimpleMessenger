using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Core.Messages;

public abstract class ChatableMessage : ITokenable
{
    public abstract MessageType MessageType { get; }
    public Token Token { get; private set; }
    public int Target { get; private set; }
    public Guid ChatHash { get; private set; }

    protected ChatableMessage() { }
    protected ChatableMessage(Token token, Guid chatHash, int target)
    {
        Token = token;
        ChatHash = chatHash;
        Target = target;
    }

    void IMessage.Write(DataWriter writer)
    {
        writer.Write(Token);
        writer.Write(ChatHash);
        writer.Write(Target);
        Write(writer);
    }
    void IMessage.Read(DataReader reader)
    {
        Token = reader.Read<Token>();
        ChatHash = reader.Read<Guid>();
        Target = reader.Read<int>();
        Read(reader);
    }

    protected abstract void Write(DataWriter writer);
    protected abstract void Read(DataReader reader);
}
