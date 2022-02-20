using SimpleMessenger.App.Infrastructure;
using SimpleMessenger.App.Model;
using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using System;
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
    public string CurrentUsername { get; set; }

    public ICommand ShowMyContactsCommand { get; }

    public HomeViewModel(IViewModelProvider provider) : base(provider)
    {
        CurrentUsername = Client.User.Name;
        Chats = new ObservableCollection<ChatModel>();

        Client.Instance.MessageReceiveEvent += Server_OnMessage;

        ChatViewer = new ChatViewerViewModel(this);
        ShowMyContactsCommand = new DelegateCommand(() =>
        {
            ShowModal(new MyContactsViewModel(this, _provider));
        });
    }

    protected override async Task InitAsync()
    {
        try
        {
            using var storage = new LocalStorage();
            var self = Client.User;

            await storage.InitAsync().ConfigureAwait(false);
            await ContactsManager.InitAsync(storage).ConfigureAwait(false);

            foreach (var chat in storage.Chats)
                Chats.Add(new ChatModel(self, chat));

            await Client.BeginSynchronizationAsync(this);
        }
        catch(Exception ex)
        {

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
            foreach (var c in ContactsManager.Contacts)
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
                _ = ContactsManager.AddAsync(contact);
            }

            var newChat = new ChatModel(new ChatMembers(Client.User, contact.User));
            newChat.TryBindToChat(msg.ChatId);
            newChat.MessageCollection.Add(msg.Message);
            Chats.Add(newChat);
        }
        else MessageBox.Show(message.ToString());
    }
}