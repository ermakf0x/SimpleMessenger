using SimpleMessenger.App.Infrastructure;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleMessenger.App.Model;

class ChatModel : ObservableObject
{
    ObservableCollection<Message> _messages;
    Chat _chat;
    string _text;

    public string Text { get => _text; set => Set(ref _text, value); }
    public ReadOnlyObservableCollection<Message> Messages { get; private set; }
    public ICommand SendMessageCommand { get; init; }
    public User User { get; }
    public User User2 { get; }


    public ChatModel(User user, User user2)
    {
        User = user;
        User2 = user2;
    }
    public ChatModel(Chat chat, User user, User user2) : this(user, user2)
    {
        _chat = chat ?? throw new ArgumentNullException(nameof(chat));
        InitMessages(_chat);
    }

    public async Task SendToChatAsync(ClientContext context)
    {
        if (_chat is null)
        {
            var msg = new CreateNewChatMessage(context.Config.Token, User2.UID, Text);
            var response = await context.Client.SendAsync(msg);
            if (response is JsonMessage json)
            {
                var hash = json.GetAs<Guid>();
                _chat = new Chat(hash);
                _chat.AddMessage(new Message
                {
                    Id = 0,
                    Content = Text
                });
                InitMessages(_chat);
                Text = "";
            }
        }
        else
        {
            var msg = new TextMessage(context.Config.Token, _chat.Hash, User2.UID, Text);
            var response = await context.Client.SendAsync(msg);
            if(response is SuccessMessage)
            {
                Text = "";
            }
        }
    }

    void InitMessages(Chat chat)
    {
        _messages = new(chat.Messages);
        Messages = new(_messages);
    }
}
