namespace SimpleMessenger.Core;

[Flags]
public enum SyncState : byte
{
    SyncSuccess = 0,
    MustSyncContacts = 1,
    MustSyncChats = 2,
    MustSyncAll = MustSyncContacts | MustSyncChats,
}