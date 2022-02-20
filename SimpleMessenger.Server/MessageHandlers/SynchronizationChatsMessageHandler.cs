using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;

class SynchronizationChatsMessageHandler : ServerMessageHandler<SynchronizationChatsMessage>
{
    protected override IResponse Process(SynchronizationChatsMessage message, ClientHandler client)
    {
        var sender = client.CurrentUser;
        if(message.SyncData.Length == 0 & sender.Chats.Count > 0)
        {
            return Json(new[]
            {
                new SyncOperation<int>
                {
                    Operation = SyncOperationType.Add,
                    Values = sender.Chats.Select(x => x.Id).ToArray(),
                }
            });
        }

        // Список чатов которые нужно удалить на клиенте
        var mustDeleted = new List<int>();
        var mustSync = new List<int>();

        var chats = new Dictionary<int, int>(sender.Chats.Select(chat => new KeyValuePair<int, int>(chat.Id, chat.Hash)));

        foreach (var sd in message.SyncData)
        {
            if (chats.TryGetValue(sd.ChatID, out var hash))
            {
                if (hash != sd.HashSum)
                    mustSync.Add(sd.ChatID);
                chats.Remove(sd.ChatID);
            }
            else
            {
                mustDeleted.Add(sd.ChatID);
            }
        }

        List<SyncOperation<int>> response = new(3);
        if (chats.Count > 0)
            response.Add(new SyncOperation<int>
            {
                Operation = SyncOperationType.Add,
                Values = chats.Values.ToArray()
            });
        if (mustSync.Count > 0)
            response.Add(new SyncOperation<int>
            {
                Operation = SyncOperationType.Update,
                Values = mustSync.ToArray()
            });
        if (mustDeleted.Count > 0)
            response.Add(new SyncOperation<int>
            {
                Operation = SyncOperationType.Remove,
                Values = mustDeleted.ToArray()
            });

        return Json(response.ToArray());
    }
}