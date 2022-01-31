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
    readonly ClientContext _context;
    int? _chatId;
    ObservableCollection<Message> _messageCollection;
    string _messageText;

    public int? ChatId { get => _chatId; private set => _chatId = value; }
    public string MessageText { get => _messageText; set => Set(ref _messageText, value); }
    public ObservableCollection<Message> MessageCollection { get => _messageCollection; private set => Set(ref _messageCollection, value); }
    public ICommand SendMessageCommand { get; }
    public ChatMembers Members { get; }

    public ChatModel(ChatMembers participants, ClientContext context, Chat? chat = null)
    {
        Members = participants ?? throw new ArgumentNullException(nameof(participants));
        _context = context ?? throw new ArgumentNullException(nameof(context));

        if(chat is null)
        {
            MessageCollection = new ObservableCollection<Message>();
        }
        else
        {
            _chatId = chat.Id;
            if (chat.Messages != null) MessageCollection = new(chat.Messages);
            else MessageCollection = new ObservableCollection<Message>();
        }

        SendMessageCommand = new AsyncCommand(SendMessageAsync, () => !string.IsNullOrEmpty(MessageText));
    }

    public void BindToChat(int chatId)
    {
        if (_chatId.HasValue) throw new InvalidOperationException("Этот чат уже был связан с ID чатом");
        ChatId = chatId;
    }

    async Task SendMessageAsync()
    {
        if (_chatId.HasValue)
        {
            var msg = new TextSMessage(_context.Config.Token, _chatId.Value, Members.Contact.UID, MessageText);
            var response = await _context.Server.SendAsync(msg);
            if (response is JsonMessage json)
            {
                MessageCollection.Add(new Message
                {
                    Id = json.GetAs<int>(),
                    Time = DateTime.Now,
                    Content = MessageText,
                    Sender = Members.Self,
                    SenderId = Members.Self.UID
                });
                MessageText = "";
            }
        }
        else
        {
            var msg = new CreateNewChatMessage(_context.Config.Token, Members.Contact.UID, MessageText);
            var response = await _context.Server.SendAsync(msg);
            if (response is JsonMessage json)
            {
                _chatId = json.GetAs<int>();
                MessageCollection.Add(new Message
                {
                    Id = 0,
                    Time = DateTime.Now,
                    Content = MessageText,
                    Sender = Members.Self,
                    SenderId = Members.Self.UID
                });
                MessageText = "";
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
}
