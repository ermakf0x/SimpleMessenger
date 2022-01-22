using SimpleMessenger.App.Infrastructure;
using SimpleMessenger.App.Model;
using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleMessenger.App.ViewModel;

class HomeViewModel : BaseViewModel
{
    readonly SMClient _client;
    readonly ClientContext _context;
    readonly ICommand _sendMessageCommand;
    ContactModel _selectedContact;
    string _username;

    public ContactModel SelectedContact { get => _selectedContact; set => Set(ref _selectedContact, value); }
    public ObservableCollection<ContactModel> Contacts { get; }
    public string Username { get => _username; set => Set(ref _username, value); }

    public ICommand FindUserCommand { get; }



    public HomeViewModel(IViewModelProvider provider, ClientContext context) : base(provider)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _client = _context.Client;

        Contacts = new ObservableCollection<ContactModel>();

        FindUserCommand = new AsyncCommand(FindUserAsync, () => !string.IsNullOrEmpty(Username));
        _sendMessageCommand = new AsyncCommand(SendMessageAsync, CanSend);
    }

    bool CanSend()
    {
        return SelectedContact is not null &&
               !string.IsNullOrEmpty(SelectedContact.Chat.Text);
    }

    Task SendMessageAsync()
    {
        var contact = SelectedContact;
        return contact.Chat.SendToChatAsync(_context);
    }

    async Task FindUserAsync()
    {
        var response = await _client.SendAsync(new FindUserMessage(Username, _context.Config.Token));

        if(response is JsonMessage json)
        {
            var user = json.GetAs<User>();
            Contacts.Add(new ContactModel
            {
                User = user,
                Chat = new ChatModel(new User
                {
                    UID = _context.Config.UID,
                }, user)
                {
                    SendMessageCommand = _sendMessageCommand
                }
            });
            Username = "";
        }
    }
}
