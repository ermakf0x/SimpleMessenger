using SimpleMessenger.Core.Model;
using System;

namespace SimpleMessenger.App.Model;

class ContactModel : ObservableObject
{
    public int UID => User.UID;
    public string Name
    {
        get => User.Name;
        set
        {
            if (value != null && value != User.Name)
            {
                User.Name = value;
                OnPropertyChanged();
            }
        }
    }
    public User User { get; }
    public ChatModel Chat { get; }

    public ContactModel(int uid, ChatModel chat)
        : this(new User { UID = uid }, chat)
    { 
        GetUserInfoAsync(uid);
    }
    public ContactModel(int uid, string name, ChatModel chat)
        : this(new User { Name = name, UID = uid }, chat) { }
    public ContactModel(User user, ChatModel chat)
    {
        User = user ?? throw new ArgumentNullException(nameof(user));
        Chat = chat ?? throw new ArgumentNullException(nameof(chat));
    }

    async void GetUserInfoAsync(int uid)
    {
        try
        {

        }
        catch
        {
            Name = "";
        }
    }

    public override string ToString() => User.ToString();
}
