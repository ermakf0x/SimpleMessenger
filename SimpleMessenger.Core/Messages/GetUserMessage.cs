using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Core.Messages;

public class GetUserMessage : BaseMessage
{
    public override MessageType MessageType => MessageType.GetUser;
    public int UID { get; private set; }

    internal GetUserMessage() { }
    public GetUserMessage(Token token, int uid) : base(token)
    {
        if (uid < 0) throw new ArgumentException("UID не может быть отрицательным", nameof(uid));
        UID = uid;
    }

    protected override void Read(DataReader reader)
    {
        UID = reader.Read<int>();
    }

    protected override void Write(DataWriter writer)
    {
        writer.Write(UID);
    }

    public override string ToString() => $"UID: {UID}";
}