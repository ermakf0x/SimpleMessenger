using SimpleMessenger.App.Infrastructure;
using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleMessenger.App.ViewModel;

class RegistrationViewModel : BaseViewModel
{
    ErrorMessage? _error;

    public string Username { get; set; }
    public string Password { get; set; }
    public string Password2 { get; set; }
    public ErrorMessage? Error { get => _error; set => Set(ref _error, value); }

    public ICommand BackCommand { get; }
    public ICommand CreateCommand { get; }

    public RegistrationViewModel(IViewModelProvider provider) : base(provider)
    {
        BackCommand = new DelegateCommand(() => _provider.Back());
        CreateCommand = new AsyncCommand(CreateAsync, CanCreated);
    }

    async Task CreateAsync()
    {
        Error = null;
        var responce = await Client.SendAsync(new RegistrationMessage(Username, Password, Username)).ConfigureAwait(false);

        if(responce is JsonMessage json)
        {
            var user = json.GetAs<MainUser>();
            Client.User.Token = user.Token;
            ConfigManager.Save(_context.Config);
            SetViewModel(new HomeViewModel(_provider), true);
            return;
        }

        if(responce is ErrorMessage err)
        {
            Error = err;
        }
    }

    bool CanCreated()
    {
        return ValidationHelper.ValidatePassword(Password) &&
               ValidationHelper.ValidateUsername(Username) &&
               Password == Password2;
    }
}
