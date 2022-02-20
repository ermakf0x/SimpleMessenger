using Microsoft.EntityFrameworkCore;
using SimpleMessenger.App.Infrastructure.Utils;
using SimpleMessenger.App.Model;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMessenger.App.Infrastructure;

sealed class ContactsManager : ObservableObject
{
    static readonly Instance<ContactsManager> s_instance = new();
    readonly ObservableCollection<ContactModel> _contacts;
    ContactModel? _contact;

    public static IEnumerable<ContactModel> Contacts => s_instance.Get()._contacts;
    public static ContactModel? SelectedContact
    {
        get => s_instance.Get()._contact;
        set
        {
            ContactsManager inst = s_instance.Get();
            inst.Set(ref inst._contact, value);
        }
    }

    private ContactsManager(IEnumerable<User> users)
    {
        _contacts = new ObservableCollection<ContactModel>(users.Select(u => new ContactModel(u)));
    }

    public static async Task InitAsync(LocalStorage storage)
    {
        await storage.Contacts.LoadAsync();
        s_instance.Set(new ContactsManager(storage.Contacts));
    }

    public static async Task AddAsync(ContactModel contact)
    {
        ArgumentNullException.ThrowIfNull(contact, nameof(contact));
        using var storage = new LocalStorage();
        await storage.AddAsync(contact);
        await storage.SaveChangesAsync();
        s_instance.Get()._contacts.Add(contact);
    }
    public static async Task AddRangeAsync(IEnumerable<ContactModel> contacts)
    {
        ArgumentNullException.ThrowIfNull(contacts, nameof(contacts));
        using var storage = new LocalStorage();
        await storage.AddRangeAsync(contacts);
        await storage.SaveChangesAsync();
        foreach (var contact in contacts)
            s_instance.Get()._contacts.Add(contact);
    }
    public static bool RemoveContact(ContactModel contact)
    {
        ArgumentNullException.ThrowIfNull(contact, nameof(contact));
        return s_instance.Get()._contacts.Remove(contact);
    }

    public static ContactModel? GetById(int id)
    {
        return s_instance.Get()._contacts.FirstOrDefault(c => c.UID == id);
    }

    public static async Task<bool> FindUserAsync(string username)
    {
        var inst = s_instance.Get();

        if (Client.User.Username == username)
            return true;
        if (inst._contacts.Where(c => c.User.Username == username).Any())
            return true;

        var response = await Client.SendAsync(new FindUserMessage(username, Client.User.Token)).ConfigureAwait(false);

        if (response is JsonMessage json)
        {
            var user = json.GetAs<User>();
            await AddAsync(new ContactModel(user));
            return true;
        }

        return false;
    }

    public override string ToString() => $"Contacts: {_contacts.Count}";
}