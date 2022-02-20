using SimpleMessenger.App.Infrastructure;
using SimpleMessenger.App.Model;
using SimpleMessenger.Core.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleMessenger.App.ViewModel;

class MyContactsViewModel : BaseModalViewModel
{
    readonly HomeViewModel _homeViewModel;
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
    public string? Username { get => _username; set => Set(ref _username, value); }
    public ICommand FindUserCommand { get; }

    public MyContactsViewModel(HomeViewModel homeViewModel, IViewModelProvider provider) : base(provider)
    {
        _homeViewModel = homeViewModel ?? throw new ArgumentNullException(nameof(homeViewModel));
        FindUserCommand = new AsyncCommand(FindUserAsync, () => !string.IsNullOrWhiteSpace(Username));
    }

    void BeginChat(User user)
    {
        var chat = _homeViewModel.Chats.FirstOrDefault(c => c.Members.Contact == user);
        if (chat is null)
        {
            chat = new ChatModel(new ChatMembers(Client.User, user));
        }
        _homeViewModel.ChatViewer.Current = chat;
        Close();
    }

    async Task FindUserAsync()
    {
        await ContactsManager.FindUserAsync(Username).ConfigureAwait(false);
        Username = "";
    }
}