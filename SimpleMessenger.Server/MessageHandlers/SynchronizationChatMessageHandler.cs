using Microsoft.EntityFrameworkCore;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;

class SynchronizationChatMessageHandler : ServerMessageHandler<SynchronizationChatMessage>
{
    protected override IResponse Process(SynchronizationChatMessage message, ClientHandler client)
    {
        var sender = client.CurrentUser;
        var chat = sender.Chats.FirstOrDefault(c => c.Id == message.ChatID);

        if (chat is null)
            return Error(ErrorMessage.ChatNotFound);

        if(message.ChunksHashSum.Length == 0)
        {

        }

        chat = client.Storage.Chats
            .Where(c => c.Id == message.ChatID)
            .Include(c => c.Chunks.Take(message.ChunksHashSum.Length))
            .FirstOrDefault();
    }
}