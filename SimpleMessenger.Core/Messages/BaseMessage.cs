using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Core.Messages;

public abstract class BaseMessage : ITokenable
{
    public abstract MessageType MessageType { get; }
    public Token Token { get; private set; }

    protected BaseMessage() { }
    public BaseMessage(Token token)
    {
        Token = token;
    }

    void IMessage.Write(DataWriter writer)
    {
        writer.Write(Token);
        Write(writer);
    }
    void IMessage.Read(DataReader reader)
    {
        Token = reader.Read<Token>();
        Read(reader);
    }

    protected abstract void Read(DataReader reader);
    protected abstract void Write(DataWriter writer);
    public override string ToString() => $"Token: {Token}";
}
