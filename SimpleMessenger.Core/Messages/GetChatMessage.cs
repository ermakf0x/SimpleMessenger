using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Core.Messages;

public class GetChatMessage : GetByIDBaseMessage
{
    public override MessageType MessageType => MessageType.GetChat;
    internal GetChatMessage() { }
    public GetChatMessage(Token token, int chatId) : base(token, chatId) { }
}
