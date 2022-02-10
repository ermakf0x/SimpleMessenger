using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;
using SimpleMessenger.Server.Model;

namespace SimpleMessenger.Server.MessageHandlers;

class SynchronizationMessageHandler : ServerMessageHandler<SynchronizationMessage>
{
    protected override IResponse Process(SynchronizationMessage message, ClientHandler client)
    {
        var syncState = Synchronization.State.MustSyncAll;
        var sender = client.CurrentUser;
        var userHash = HashCodeCombiner.Combine(sender.Contacts);
        var chatHash = HashCodeCombiner.Combine(sender.Chats);

        //TODO: Возможно стоило бы переписать без 'if'
        if (message.UserHash == userHash)
            syncState &= Synchronization.State.MustSyncChats;
        if (message.ChatHash == chatHash)
            syncState &= Synchronization.State.MustSyncContacts;

        return Json(syncState);
    }

    static int CalculateUserHash(ICollection<User2> users)
    {
        var hash = new HashCode();
        foreach (var user in users)
            hash.Add(user);
        return hash.ToHashCode();
    }

    static int CalculateChatHash(ICollection<Chat> chats)
    {
        var hash = new HashCode();
        foreach (var chat in chats)
            hash.Add(chat);
        return hash.ToHashCode();
    }
}