using SimpleMessenger.App.Infrastructure;
using SimpleMessenger.App.Model;
using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SimpleMessenger.App.ViewModel;

class HomeViewModel : BaseViewModel
{
    readonly ClientContext _context;
    ContactModel _selectedContact;
    string _username;

    public ContactModel SelectedContact { get => _selectedContact; set => Set(ref _selectedContact, value); }
    public ObservableCollection<ContactModel> Contacts { get; }
    public string Username { get => _username; set => Set(ref _username, value); }
    public string CurrentUsername { get; set; }

    public ICommand FindUserCommand { get; }

    public HomeViewModel(IViewModelProvider provider, ClientContext context) : base(provider)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        Contacts = new ObservableCollection<ContactModel>();
        CurrentUsername = context.Config.Name;

        FindUserCommand = new AsyncCommand(FindUserAsync, () => !string.IsNullOrEmpty(Username));

        _context.Server.BeginReceiveMessages();
        _context.Server.OnMessage += Server_OnMessage;
    }

    void Server_OnMessage(IMessage message)
    {
        if (message is TextMessage msg)
        {
            foreach (var contact in Contacts)
            {
                if(contact.Chat.ChatID.HasValue && contact.Chat.ChatID == msg.ChatID)
                {
                    contact.Chat.MessageCollection.Add(new Message(contact.User, msg.Content));
                    return;
                }
            }
            foreach (var contact in Contacts)
            {
                if(contact.User.UID == msg.Sender)
                {
                    contact.Chat.BindToChat(msg.ChatID);
                    contact.Chat.MessageCollection.Add(new Message(contact.User, msg.Content));
                    return;
                }
            }
            var user = new User() { UID = msg.Sender };
            var newChat = new ChatModel(new ChatParticipants(_context.Config, user), _context);
            newChat.BindToChat(msg.ChatID);
            newChat.MessageCollection.Add(new Message(user, msg.Content));
            var newContact = new ContactModel(user, newChat);
            Contacts.Add(newContact);

            MessageBox.Show("Нет подходящего чата");
        }
        else MessageBox.Show(message.ToString());
    }

    async Task FindUserAsync()
    {
        var response = await _context.Server.SendAsync(new FindUserMessage(Username, _context.Config.Token));

        if(response is JsonMessage json)
        {
            var user = json.GetAs<User>();
            var chat = new ChatModel(new ChatParticipants(_context.Config, user), _context);
            Contacts.Add(new ContactModel(user, chat));
            Username = "";
        }
    }
}
