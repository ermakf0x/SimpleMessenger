using SimpleMessenger.App.Infrastructure;
using SimpleMessenger.Core.Model;

namespace SimpleMessenger.App.Model;

class ContactModel
{
    public User User { get; init; }
    public ChatModel Chat { get; init; }
}
