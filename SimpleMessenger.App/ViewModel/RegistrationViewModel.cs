using System.Windows.Input;

namespace SimpleMessenger.App.ViewModel;

class RegistrationViewModel : ViewModelBase
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Password2 { get; set; }
    public IAsyncCommand Execute { get; set; }


    public RegistrationViewModel(IViewModelProvider provider) : base(provider)
    {
    }
}
