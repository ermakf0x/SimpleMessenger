namespace SimpleMessenger.Core.Messages;

public class GetUsersMessage : EmptyMessage
{
    public override MessageType MessageType => MessageType.GetUsers;
}
