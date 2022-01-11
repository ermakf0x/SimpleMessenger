using SimpleMessenger.Core;

namespace SimpleMessenger.Server.MessageHandlers;
class GetUsersMessageHandler : ServerMessageHandlerBase
{
    protected override IResponse Process(IMessage message, ServerClient client)
    {
        if (!message.IsAuth(client)) return Error(ErrorMsgHelper.NotAuthorized);

        var users = LocalDB.GetUsers().Where(u => u.Data.Id != client.User.Data.Id).Select(u => u.Data).ToList();
        return Content(users);
    }
}

