using System.Windows.Input;

namespace SimpleMessenger.App.ViewModel;

class AuthorizationViewModel : ViewModelBase
{
    public string Username { get; set; }
    public string Password { get; set; }
    public IAsyncCommand Execute { get; set; }

    public AuthorizationViewModel(IViewModelProvider provider) : base(provider)
    {
    }
}
