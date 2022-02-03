using SimpleMessenger.App.Infrastructure;
using SimpleMessenger.App.Model;
using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SimpleMessenger.App.ViewModel;

class HomeViewModel : BaseViewModel
{
    readonly ClientContext _context;

    public ChatViewerViewModel ChatViewer { get; }
    public ObservableCollection<ChatModel> Chats { get; }
    public ObservableCollection<ContactModel> Contacts { get; }
    public string CurrentUsername { get; set; }

    public ICommand ShowMyContactsCommand { get; }

    public HomeViewModel(IViewModelProvider provider, ClientContext context) : base(provider)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        using (var ls = new LocalStorage())
        {
            var self = _context.Config;
            var chats = ls.Chats.Select(chat => new ChatModel(new ChatMembers(self, chat.Members.Where(user => user != self).First()), chat)).ToList();
            Contacts = new ObservableCollection<ContactModel>(ls.Contacts.Select(u => new ContactModel(u, _context)).ToList());
            Chats = new ObservableCollection<ChatModel>(chats);
        }

        CurrentUsername = context.Config.Name;

        _context.Server.BeginReceiveMessages();
        _context.Server.NewMessageReceived += Server_OnMessage;

        ChatViewer = new ChatViewerViewModel(this, context);
        ShowMyContactsCommand = new DelegateCommand(() =>
        {
            ShowModal(new MyContactsViewModel(this, _context, _provider));
        });
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
                contact = new ContactModel(msg.Sender, _context);
                Contacts.Add(contact);
            }

            var newChat = new ChatModel(new ChatMembers(_context.Config, contact.User));
            newChat.TryBindToChat(msg.ChatId);
            newChat.MessageCollection.Add(msg.Message);
            Chats.Add(newChat);
        }
        else MessageBox.Show(message.ToString());
    }
}