using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Core;

public static class Extensions
{
    #region SyncStateExtensions

    public static bool MustSync(this SyncState state) => state != SyncState.SyncSuccess;
    public static bool MustSyncContacts(this SyncState state) => (state & SyncState.MustSyncContacts) == SyncState.MustSyncContacts;
    public static bool MustSyncChats(this SyncState state) => (state & SyncState.MustSyncChats) == SyncState.MustSyncChats;
    public static bool MustSyncAll(this SyncState state) => state == SyncState.MustSyncAll;

    #endregion

    public static bool IsPrivate(this Chat chat)
    {
        ArgumentNullException.ThrowIfNull(chat, nameof(chat));
        return chat.FirstUserID == chat.SecondUserID && chat.FirstUserID != -1;
    }
    public static bool IsMember(this Chat chat, User user)
    {
        ArgumentNullException.ThrowIfNull(chat, nameof(chat));
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        return chat.FirstUserID == user.UID || chat.SecondUserID == user.UID;
    }
}