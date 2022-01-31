using SimpleMessenger.App.Infrastructure;
using SimpleMessenger.App.Model;
using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        _context.Server.NewMessageReceived += Server_OnMessage;
    }

    void Server_OnMessage(IMessage message)
    {
        if (message is TextMessage msg)
        {
            if(Contacts.Count > 0)
            {
                foreach (var contact in Contacts)
                {
                    if (contact.Chat.ChatId.HasValue && contact.Chat.ChatId == msg.ChatId)
                    {
                        contact.Chat.MessageCollection.Add(msg.Message);
                        return;
                    }
                }
                foreach (var contact in Contacts)
                {
                    if (contact.User.UID == msg.Sender)
                    {
                        contact.Chat.BindToChat(msg.ChatId);
                        contact.Chat.MessageCollection.Add(msg.Message);
                        return;
                    }
                }
            }

            var user = new User() { UID = msg.Sender };
            var newChat = new ChatModel(new ChatMembers(_context.Config, user), _context);
            newChat.BindToChat(msg.ChatId);
            newChat.MessageCollection.Add(msg.Message);
            var newContact = new ContactModel(user, newChat, _context);
            Contacts.Add(newContact);
        }
        else MessageBox.Show(message.ToString());
    }

    async Task FindUserAsync()
    {
        var response = await _context.Server.SendAsync(new FindUserMessage(Username, _context.Config.Token));

        if(response is JsonMessage json)
        {
            var user = json.GetAs<User>();
            if (!Contacts.Where(c => c.UID == user.UID).Any())
            {
                var chat = new ChatModel(new ChatMembers(_context.Config, user), _context);
                Contacts.Add(new ContactModel(user, chat, _context));
            }
            Username = "";
        }
    }
}
