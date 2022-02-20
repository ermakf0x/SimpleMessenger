using Microsoft.EntityFrameworkCore;
using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;

class GetChatMessageHandler : ServerMessageHandler<GetChatMessage>
{
    protected override IResponse Process(GetChatMessage message, ClientHandler client)
    {
        var sender = client.CurrentUser;

        if(sender.Chats.Where(a => a.Id == message.Id).FirstOrDefault() is not null)
        {
            var result = client.Storage.Chats.Where(a => a.Id == message.Id)
                                             .Include(a => a.Chunks)
                                             //.ThenInclude(a => a.Messages)
                                             .FirstOrDefault();
            if(result is not null)
            {
                return Json(result);
            }
        }

        return Error(ErrorMessage.ChatNotFound);
    }
}
