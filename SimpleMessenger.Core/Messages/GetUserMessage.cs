using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Core.Messages;

public class GetUserMessage : GetByIDBaseMessage
{
    public override MessageType MessageType => MessageType.GetUser;
    internal GetUserMessage() { }
    public GetUserMessage(Token token, int uid) : base(token, uid) { }
}