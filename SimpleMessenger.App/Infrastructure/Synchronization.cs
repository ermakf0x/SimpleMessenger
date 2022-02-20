using SimpleMessenger.App.Model;
using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMessenger.App.Infrastructure;

class Synchronization
{
    readonly NetworkChannel _channel;
    readonly MainUser _user;
    readonly ICollection<ChatModel> _chats;

    public Synchronization(NetworkChannel channel, MainUser user, ICollection<ChatModel> chats)
    {
        _channel = channel ?? throw new ArgumentNullException(nameof(channel));
        _user = user ?? throw new ArgumentNullException(nameof(user));
        _chats = chats ?? throw new ArgumentNullException(nameof(chats));
    }

    public async Task<bool> BeginSynchronizationAsync()
    {
        try
        {
        sync:
            var contacts = ContactsManager.Contacts.Select(c => c.User);
            var chats = Array.Empty<Chat>();// _chats.Select(c => c.Base);
            var message = new SynchronizationMessage(_user.Token, contacts, chats);
            var response = await SendAsync(message);
            if (response is JsonMessage json)
            {
                var state = json.GetAs<SyncState>();
                if (state.MustSync())
                {
                    if (state.MustSyncContacts())
                        await SynchronizationContactsAsync();
                    if (state.MustSyncChats())
                        await SynchronizationChatsAsync();
                    //goto sync;
                }
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    #region SynchronizationContacts

    async Task SynchronizationContactsAsync()
    {
        var contacts = ContactsManager.Contacts.Select(c => c.User);
        var response = await SendAsync(new SynchronizationContactsMessage(_user.Token, contacts));
        if(response is JsonMessage json)
        {
            var operations = json.GetAs<SyncOperation<int>[]>();
            foreach (var operation in operations)
            {
                switch (operation.Operation)
                {
                    case SyncOperationType.Add:
                        await AddUsersAsync(operation.Values);
                        break;
                    case SyncOperationType.Remove:
                        await RemoveUsersAsync(operation.Values);
                        break;

                    case SyncOperationType.Update:
                    default: continue;
                }
            }
        }
    }
    async Task AddUsersAsync(int[] uids)
    {
        if (uids.Length == 0) return;

        var users = new List<User>(uids.Length);
        var message = new GetUserMessage(_user.Token, 0);
        IMessage response;

        foreach (var uid in uids)
        {
            message.Id = uid;
            response = await SendAsync(message);
            if(response is JsonMessage json)
            {
                users.Add(json.GetAs<User>());
            }
        }

        if(users.Count > 0)
        {
            _ = App.Current.Dispatcher.InvokeAsync(async () =>
            {
                foreach (var user in users)
                {
                    await ContactsManager.AddAsync(new(user));
                }
            });
            using var storage = new LocalStorage();
            storage.Contacts.AddRange(users);
            await storage.SaveChangesAsync();
        }
    }
    async Task RemoveUsersAsync(int[] uids)
    {
        if(uids.Length == 0) return;

        _ = App.Current.Dispatcher.InvokeAsync(() =>
        {
            foreach (var uid in uids)
            {
                var user = ContactsManager.Contacts.FirstOrDefault(contact => contact.UID == uid);
                if(user is not null)
                    ContactsManager.RemoveContact(user);
            }
        });

        using var storage = new LocalStorage();
        foreach (var uid in uids)
        {
            var user = storage.Contacts.FirstOrDefault(contact => contact.UID == uid);
            if (user is not null)
                storage.Contacts.Remove(user);
        }
        await storage.SaveChangesAsync();
    }

    #endregion

    #region SynchronizationChats

    async Task SynchronizationChatsAsync()
    {
        var chats = Array.Empty<Chat>();//_chats.Select(a => a.Base);
        var response = await SendAsync(new SynchronizationChatsMessage(_user.Token, chats));
        if (response is JsonMessage json)
        {
            var operations = json.GetAs<SyncOperation<int>[]>();
            foreach (var operation in operations)
            {
                switch (operation.Operation)
                {
                    case SyncOperationType.Add:
                        await AddChatsAsync(operation.Values);
                        break;

                    case SyncOperationType.Remove:
                        await RemoveUsersAsync(operation.Values);
                        break;

                    case SyncOperationType.Update:
                        await UpdateChatAsync(operation.Values);
                        break;

                    default: continue;
                }
            }
        }
    }

    async Task UpdateChatAsync(int[] chatsID)
    {
        if (chatsID.Length == 0) return;

        foreach (var id in chatsID)
        {
            var chat = _chats.Where(c => c.ChatId == id).First().Base;
            var response = await SendAsync(new SynchronizationChatMessage(_user.Token, chat));
            if (response is JsonMessage json)
            {

            }
        }
    }

    async Task AddChatsAsync(int[] chatsID)
    {
        try
        {
            if (chatsID.Length == 0) return;

            var chats = new List<Chat>(chatsID.Length);
            var message = new GetChatMessage(_user.Token, 0);
            IMessage response;
            using var storage = new LocalStorage();

            foreach (var id in chatsID)
            {
                message.Id = id;
                response = await SendAsync(message);
                if (response is JsonMessage json)
                {
                    var chat = json.GetAs<Chat>();
                    chats.Add(chat);
                    storage.Chats.Add(chat);
                }
            }
            await storage.SaveChangesAsync();

            if (chats.Count > 0)
            {
                _ = App.Current.Dispatcher.InvokeAsync(() =>
                {
                    foreach (var chat in chats)
                    {
                        _chats.Add(new(_user, chat));
                    }
                });
                //using var storage = new LocalStorage();
                //storage.Chats.AddRange(chats);
                //var users = chats.Select(chat => chat.Members.First(a => a != _user)).ToList();
                //storage.Contacts.AttachRange(users);
                //await storage.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {

        }
    }

    #endregion

    async Task<IMessage> SendAsync(IMessage message)
    {
        await _channel.SendAsync(message).ConfigureAwait(false);
        return await TryReceiveAsync().ConfigureAwait(false);
    }
    async Task<IMessage> TryReceiveAsync()
    {
        IMessage? message;

        while (true)
        {
            message = await _channel.ReceiveAsync().ConfigureAwait(false);
            if (message is not null) return message;
            await Task.Delay(1).ConfigureAwait(false);
        }
    }
}