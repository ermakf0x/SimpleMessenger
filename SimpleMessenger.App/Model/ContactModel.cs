using SimpleMessenger.App.Infrastructure;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;
using System;

namespace SimpleMessenger.App.Model;

class ContactModel : ObservableObject
{
    public int UID => User.UID;
    public string Name
    {
        get => User.Name ?? "user";
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
    public ChatModel? Chat { get; set; }


    public ContactModel(int uid) : this(new User { UID = uid })
    { 
        GetUserInfoAsync(uid);
    }
    public ContactModel(int uid, string name) : this(new User { Name = name, UID = uid }) { }
    public ContactModel(User user)
    {
        User = user ?? throw new ArgumentNullException(nameof(user));

        if(User.Name == null)
            GetUserInfoAsync(User.UID);
    }

    async void GetUserInfoAsync(int uid)
    {
        try
        {
            var response = await Client.SendAsync(new GetUserMessage(Client.User.Token, uid));
            if (response is JsonMessage json)
            {
                var user = json.GetAs<User>();
                Name = user.Name;
            }
        }
        catch { }
    }

    public override string ToString() => Name;
}
