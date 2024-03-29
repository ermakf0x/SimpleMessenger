﻿using SimpleMessenger.App.Infrastructure;
using SimpleMessenger.App.Model;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleMessenger.App.ViewModel;

class ChatViewerViewModel : ObservableObject
{
    readonly HomeViewModel _homeViewModel;
    string? _messageString;
    ChatModel? _current;

    public string? MessageString { get => _messageString; set => Set(ref _messageString, value); }
    public ChatModel? Current { get => _current; set => Set(ref _current, value); }
    
    public ICommand SendMessageCommand { get; }

    public ChatViewerViewModel(HomeViewModel homeViewModel)
    {
        _homeViewModel = homeViewModel ?? throw new ArgumentNullException(nameof(homeViewModel));
        SendMessageCommand = new AsyncCommand(SendMessageAsync, () => Current is not null && !string.IsNullOrWhiteSpace(MessageString));
    }

    async Task SendMessageAsync()
    {
        var chat = Current;
        if (chat.ChatId >= 0)
        {
            var msg = new TextSMessage(Client.User.Token, chat.ChatId, chat.Members.Contact.UID, MessageString);
            var response = await Client.SendAsync(msg);
            if (response is JsonMessage json)
            {
                chat.MessageCollection.Add(new Message
                {
                    Id = json.GetAs<int>(),
                    Time = TimeOnly.FromDateTime(DateTime.Now),
                    Content = MessageString,
                    Sender = chat.Members.Self,
                    SenderId = chat.Members.Self.UID
                });
                MessageString = "";
            }
        }
        else
        {
            var msg = new CreateNewChatMessage(Client.User.Token, chat.Members.Contact.UID, MessageString);
            var response = await Client.SendAsync(msg);//.ConfigureAwait(false);
            if (response is JsonMessage json)
            {
                if (chat.TryBindToChat(json.GetAs<int>()))
                    _homeViewModel.Chats.Add(chat);
                chat.MessageCollection.Add(new Message
                {
                    Id = 0,
                    Time = TimeOnly.FromDateTime(DateTime.Now),
                    Content = MessageString,
                    Sender = chat.Members.Self,
                    SenderId = chat.Members.Self.UID
                });
                MessageString = "";

            }
        }
    }
}
