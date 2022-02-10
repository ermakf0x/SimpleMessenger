using SimpleMessenger.Core.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace SimpleMessenger.App.Model;

class ChatModel : ObservableObject
{
    Message? _lastMessage;

    public int ChatId { get; private set; } = -1;
    public Message? LastMessage { get => _lastMessage; private set => Set(ref _lastMessage, value); }
    public ObservableCollection<Message> MessageCollection { get; }
    public ChatMembers Members { get; }

    public ChatModel(ChatMembers members)
    {
        Members = members ?? throw new ArgumentNullException(nameof(members));
        
        MessageCollection = new ObservableCollection<Message>();
        MessageCollection.CollectionChanged += MessageCollection_CollectionChanged;
    }
    public ChatModel(User self, Chat chat)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(chat, nameof(chat));

        Members = new ChatMembers(self, chat.Members);

        MessageCollection = new();// new ObservableCollection<Message>(chat.Messages);
        _lastMessage = MessageCollection.LastOrDefault();
        ChatId = chat.Id;
        MessageCollection.CollectionChanged += MessageCollection_CollectionChanged;
    }

    public bool TryBindToChat(int chatId)
    {
        if (chatId < 0) throw new InvalidOperationException("Id чата не может быть отрицательный числом");
        if (ChatId != -1) return false;
        ChatId = chatId;
        return true;
    }

    void MessageCollection_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if(e.Action == NotifyCollectionChangedAction.Add)
        {
            if(e.NewItems is not null)
            {
                LastMessage = e.NewItems[^1] as Message;
            }
        }
    }
}

class ChatMembers
{
    public User Self { get; }
    public User Contact { get; }

    public ChatMembers(User self, User contact)
    {
        Self = self ?? throw new ArgumentNullException(nameof(self));
        Contact = contact ?? throw new ArgumentNullException(nameof(contact));
    }
    public ChatMembers(User self, IEnumerable<User> members)
    {
        ArgumentNullException.ThrowIfNull(members, nameof(members));
        Self = self;
        Contact = members.FirstOrDefault(u => u != self) ?? throw new ArgumentException();
    }
}
