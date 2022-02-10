using SimpleMessenger.App.Infrastructure;
using SimpleMessenger.App.Model;
using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SimpleMessenger.App.ViewModel;

class HomeViewModel : BaseViewModel
{
    public ChatViewerViewModel ChatViewer { get; }
    public ObservableCollection<ChatModel> Chats { get; }
    public ObservableCollection<ContactModel> Contacts { get; }
    public string CurrentUsername { get; set; }

    public ICommand ShowMyContactsCommand { get; }

    public HomeViewModel(IViewModelProvider provider) : base(provider)
    {
        CurrentUsername = Client.User.Name;
        Chats = new ObservableCollection<ChatModel>();
        Contacts = new ObservableCollection<ContactModel>();

        Client.Instance.MessageReceiveEvent += Server_OnMessage;

        ChatViewer = new ChatViewerViewModel(this);
        ShowMyContactsCommand = new DelegateCommand(() =>
        {
            ShowModal(new MyContactsViewModel(this, _provider));
        });
    }

    protected override async Task InitAsync()
    {
        using var storage = new LocalStorage();
        var self = Client.User;
        await storage.InitAsync().ConfigureAwait(false);

        var chats = storage.Chats.ToList();
        var contacts = storage.Contacts.ToList();

        foreach (var chat in chats)
            Chats.Add(new ChatModel(self, chat));
        foreach (var user in contacts)
            Contacts.Add(new ContactModel(user));

        var response = await Client.SendAsync(new SynchronizationMessage(self.Token, contacts, chats));
        if(response is JsonMessage json)
        {
            var syncState = json.GetAs<Synchronization.State>();
        }
    }

    void Server_OnMessage(IMessage message)
    {
        if (message is TextMessage msg)
        {
            if(ChatViewer.Current != null && ChatViewer.Current.ChatId == msg.ChatId)
            {
                ChatViewer.Current.MessageCollection.Add(msg.Message);
                if (ChatViewer.Current.ChatId == -1)
                {
                    if (ChatViewer.Current.TryBindToChat(msg.ChatId))
                        Chats.Add(ChatViewer.Current);
                }
                return;
            }

            foreach (var chat in Chats)
            {
                if (chat.ChatId == msg.ChatId)
                {
                    chat.MessageCollection.Add(msg.Message);
                    return;
                }
            }

            ContactModel? contact = null;
            foreach (var c in Contacts)
            {
                if(c.UID == msg.Sender)
                {
                    contact = c;
                    break;
                }
            }

            if (contact is null)
            {
                contact = new ContactModel(msg.Sender);
                Contacts.Add(contact);
            }

            var newChat = new ChatModel(new ChatMembers(Client.User, contact.User));
            newChat.TryBindToChat(msg.ChatId);
            newChat.MessageCollection.Add(msg.Message);
            Chats.Add(newChat);
        }
        else MessageBox.Show(message.ToString());
    }
}