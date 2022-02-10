using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;

class SynchronizationContactsMessageHandler : ServerMessageHandler<SynchronizationContactsMessage>
{
    protected override IResponse Process(SynchronizationContactsMessage message, ClientHandler client)
    {
        var sender = client.CurrentUser;
        if(message.UIDs?.Length == 0 && sender.Contacts.Count > 0)
        {
            return Json(sender.Contacts.Select(contact => contact.GetAsUser()).ToArray());
        }

        // Список users которые нужно удалить на клиенте
        var mustDeleted = new List<int>();

        var contacts = new HashSet<int>(sender.Contacts.Select(user => user.UID));

        foreach (var uid in message.UIDs)
        {
            if(contacts.Contains(uid))
            {
                contacts.Remove(uid);
            }
            else
            {
                mustDeleted.Add(uid);
            }
        }

        return Success();
    }
}