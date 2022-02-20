using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Core.Messages;

public abstract class GetByIDBaseMessage : BaseMessage
{
    int _id;
    public int Id
    {
        get => _id;
        set
        {
            if (value < 0) throw new InvalidOperationException("Id не может быть отрицательным");
            _id = value;
        }
    }

    protected GetByIDBaseMessage() { }
    public GetByIDBaseMessage(Token token, int id) : base(token)
    {
        Id = id;
    }

    protected override void Read(DataReader reader)
    {
        Id = reader.Read<int>();
    }
    protected override void Write(DataWriter writer)
    {
        writer.Write(Id);
    }

    public override string ToString() => $"Id: {Id}";
}
