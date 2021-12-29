using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;
class GetUsersHandler : IMessageHandler<ServerClient>
{
    public void Process(ServerClient client)
    {
        Helper.WriteMessageAsync(client.Stream, client.MessageSerializer,
            new ResponseUsersMessage(LocalDB.GetUsers().Where(u => u.Data.Id != client.User.Data.Id).Select(u => u.Data).ToList()));
    }
}

