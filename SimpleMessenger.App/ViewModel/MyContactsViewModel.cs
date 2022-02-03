using SimpleMessenger.App.Infrastructure;
using SimpleMessenger.App.Model;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleMessenger.App.ViewModel;

class MyContactsViewModel : BaseModalViewModel
{
    readonly HomeViewModel _homeViewModel;
    readonly ClientContext _context;
    ContactModel _selectedContact;
    string? _username;

    public ContactModel SelectedContact
    {
        get => _selectedContact;
        set
        {
            _selectedContact = value;
            if (_selectedContact is not null)
                BeginChat(_selectedContact.User);
        }
    }
    public ObservableCollection<ContactModel> Contacts { get; }
    public string? Username { get => _username; set => Set(ref _username, value); }
    public ICommand FindUserCommand { get; }

    public MyContactsViewModel(HomeViewModel homeViewModel, ClientContext context, IViewModelProvider provider)
        : base(provider)
    {
        _homeViewModel = homeViewModel ?? throw new ArgumentNullException(nameof(homeViewModel));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        Contacts = homeViewModel.Contacts;
        FindUserCommand = new AsyncCommand(FindUserAsync, () => !string.IsNullOrEmpty(Username));
    }

    void BeginChat(User user)
    {
        var chat = _homeViewModel.Chats.FirstOrDefault(c => c.Members.Contact == user);
        if (chat is null)
        {
            chat = new ChatModel(new ChatMembers(_context.Config, user));
            _homeViewModel.Chats.Add(chat);
        }
        _homeViewModel.ChatViewer.Current = chat;
        Close();
    }

    async Task FindUserAsync()
    {
        if (Contacts.Where(c => c.User.Username == Username).Any())
        {
            Username = "";
            return;
        }

        var response = await _context.Server.SendAsync(new FindUserMessage(Username, _context.Config.Token)).ConfigureAwait(false);

        if (response is JsonMessage json)
        {
            var user = json.GetAs<User>();
            using (var ls = new LocalStorage())
            {
                ls.Contacts.Add(user);
                await ls.SaveChangesAsync().ConfigureAwait(false);
            }
            Contacts.Add(new ContactModel(user, _context));
            Username = "";
        }
    }
}