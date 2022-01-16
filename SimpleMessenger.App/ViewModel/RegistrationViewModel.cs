using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using System;
using System.Threading.Tasks;

namespace SimpleMessenger.App.ViewModel;

class RegistrationViewModel : ViewModelBase
{
    readonly SMClient _client;

    public string Username { get; set; }
    public string Password { get; set; }
    public string Password2 { get; set; }
    public IAsyncCommand Execute { get; }


    public RegistrationViewModel(IViewModelProvider provider, SMClient client) : base(provider)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        Execute = new AsyncCommand(ExecuteMethod);
    }

    async Task ExecuteMethod()
    {
        var responce = await _client.SendAsync(new RegistrationMessage(Username, Password, Username));

    }
}
