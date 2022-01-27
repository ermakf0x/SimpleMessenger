﻿using SimpleMessenger.App.Infrastructure;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;
using System;

namespace SimpleMessenger.App.Model;

class ContactModel : ObservableObject
{
    ClientContext _context;
    
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
    public ChatModel Chat { get; }


    public ContactModel(int uid, ChatModel chat, ClientContext context)
        : this(new User { UID = uid }, chat, context)
    { 
        GetUserInfoAsync(uid);
    }
    public ContactModel(int uid, string name, ChatModel chat, ClientContext context)
        : this(new User { Name = name, UID = uid }, chat, context) { }
    public ContactModel(User user, ChatModel chat, ClientContext context)
    {
        User = user ?? throw new ArgumentNullException(nameof(user));
        Chat = chat ?? throw new ArgumentNullException(nameof(chat));
        _context = context ?? throw new ArgumentNullException(nameof(context));

        if(User.Name == null)
            GetUserInfoAsync(User.UID);
    }

    async void GetUserInfoAsync(int uid)
    {
        try
        {
            var response = await _context.Server.SendAsync(new GetUserMessage(_context.Config.Token, uid));
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