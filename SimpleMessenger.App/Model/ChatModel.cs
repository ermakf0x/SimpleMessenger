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
    Guid? _chatID;
    ObservableCollection<Message> _messageCollection;
    string _messageText;

    public Guid? ChatID { get => _chatID; private set => _chatID = value; }
    public string MessageText { get => _messageText; set => Set(ref _messageText, value); }
    public ObservableCollection<Message> MessageCollection { get => _messageCollection; private set => Set(ref _messageCollection, value); }
    public ICommand SendMessageCommand { get; }
    public ChatParticipants Participants { get; }

    public ChatModel(ChatParticipants participants, ClientContext context, Chat? chat = null)
    {
        Participants = participants ?? throw new ArgumentNullException(nameof(participants));
        _context = context ?? throw new ArgumentNullException(nameof(context));

        if(chat is null)
        {
            MessageCollection = new ObservableCollection<Message>();
        }
        else
        {
            _chatID = chat.ChatID;
            if (chat.Messages != null) MessageCollection = new(chat.Messages);
            else MessageCollection = new ObservableCollection<Message>();
        }

        SendMessageCommand = new AsyncCommand(SendMessageAsync, () => !string.IsNullOrEmpty(MessageText));
    }

    public void BindToChat(Guid chatID)
    {
        if (_chatID.HasValue) throw new InvalidOperationException("Этот чат уже был связан с ID чатом");
        ChatID = chatID;
    }

    async Task SendMessageAsync()
    {
        if (_chatID.HasValue)
        {
            var msg = new TextSMessage(_context.Config.Token, _chatID.Value, Participants.Contact.UID, MessageText);
            var response = await _context.Server.SendAsync(msg);
            if (response is SuccessMessage)
            {
                AddMessage(MessageText);
                MessageText = "";
            }
        }
        else
        {
            var msg = new CreateNewChatMessage(_context.Config.Token, Participants.Contact.UID, MessageText);
            var response = await _context.Server.SendAsync(msg);
            if (response is JsonMessage json)
            {
                _chatID = json.GetAs<Guid>();
                AddMessage(MessageText);
                MessageText = "";
            }
        }
    }

    void AddMessage(string msg) => MessageCollection.Add(new Message(Participants.Self, msg));
}

class ChatParticipants
{
    public User Self { get; }
    public User Contact { get; }

    public ChatParticipants(User self, User contact)
    {
        Self = self ?? throw new ArgumentNullException(nameof(self));
        Contact = contact ?? throw new ArgumentNullException(nameof(contact));
    }
}
