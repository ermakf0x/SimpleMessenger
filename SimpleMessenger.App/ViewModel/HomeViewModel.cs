using SimpleMessenger.App.Model;
using SimpleMessenger.Core;
using SimpleMessenger.Core.Model;
using System;
using System.Collections.ObjectModel;

namespace SimpleMessenger.App.ViewModel;

class HomeViewModel : ViewModelBase
{
    readonly SMClient _client;
    readonly ClientContext _context;
    ChatViewModel _selectedChat;

    public ChatViewModel SelectedChat { get => _selectedChat; set => Set(ref _selectedChat, value); }
    public ObservableCollection<ChatViewModel> Chats { get; }


    public HomeViewModel(IViewModelProvider provider, ClientContext context) : base(provider)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _client = _context.Client;

        Chats = new ObservableCollection<ChatViewModel>()
        {
            new ChatViewModel(new Chat(Guid.NewGuid())),
            new ChatViewModel(new Chat(Guid.NewGuid())),
            new ChatViewModel(new Chat(Guid.NewGuid())),
        };
    }
}

class ChatViewModel : ObservableObject
{
    readonly Chat _chat;
    readonly ObservableCollection<Message> _messages;
    string _text;

    public string Text { get => _text; set => Set(ref _text, value); }
    public ReadOnlyObservableCollection<Message> Messages { get; }

    public ChatViewModel(Chat chat)
    {
        _chat = chat ?? throw new ArgumentNullException(nameof(chat));

        _messages = new(_chat.Messages);
        Messages = new(_messages);
    }
}
