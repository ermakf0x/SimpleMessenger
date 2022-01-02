using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;
class GetUsersHandler : ServerMessageHandlerBase
{
    protected override void Process(IMessage message, ServerClient client)
    {
        var users = LocalDB.GetUsers().Where(u => u.Data.Id != client.User.Data.Id).Select(u => u.Data).ToList();
        client.SendAsync(new ResponseUsersMessage(users));
    }
}

