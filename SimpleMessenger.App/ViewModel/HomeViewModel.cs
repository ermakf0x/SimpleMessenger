using SimpleMessenger.Core;
using System;

namespace SimpleMessenger.App.ViewModel;

class HomeViewModel : ViewModelBase
{
    readonly SMClient _client;

    public HomeViewModel(IViewModelProvider provider, SMClient client) : base(provider)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }
}
