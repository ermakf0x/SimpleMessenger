namespace SimpleMessenger.Core.Messages;

public class SuccessMessage : EmptyMessage, IResponse
{
    public override MessageType MessageType => MessageType.Success;
}
