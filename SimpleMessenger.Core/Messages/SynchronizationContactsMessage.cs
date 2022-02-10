using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Core.Messages;

public class SynchronizationContactsMessage : BaseMessage
{
    public override MessageType MessageType => MessageType.SynchronizationContacts;

    public int[]? UIDs { get; private set; }

    internal SynchronizationContactsMessage() { }
    public SynchronizationContactsMessage(Token token, IEnumerable<User> contacts) : base(token)
    {
        UIDs = contacts?.Select(x => x.UID).ToArray();
    }

    protected override void Read(DataReader reader)
    {
        UIDs = reader.ReadArray<int>();
    }
    protected override void Write(DataWriter writer)
    {
        writer.Write(UIDs);
    }
}