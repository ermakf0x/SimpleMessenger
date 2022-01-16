using SimpleMessenger.Core;
using System;

namespace SimpleMessenger.App.ViewModel;

class AuthorizationViewModel : ViewModelBase
{
    readonly SMClient _client;

    public string Username { get; set; }
    public string Password { get; set; }
    public IAsyncCommand Execute { get; }

    public AuthorizationViewModel(IViewModelProvider provider, SMClient client) : base(provider)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }
}
