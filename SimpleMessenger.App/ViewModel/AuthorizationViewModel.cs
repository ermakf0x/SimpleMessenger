using SimpleMessenger.App.Infrastructure;
using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleMessenger.App.ViewModel;

class AuthorizationViewModel : BaseViewModel
{
    ErrorMessage? _error;

    public string Username { get; set; }
    public string Password { get; set; }
    public ErrorMessage? Error { get => _error; set => Set(ref _error, value); }

    public ICommand CreateNewCommand { get; }
    public IAsyncCommand AuthCommand { get; }

    public AuthorizationViewModel(IViewModelProvider provider) : base(provider)
    {
        CreateNewCommand = new DelegateCommand(CreateNew);
        AuthCommand = new AsyncCommand(AuthAsync, HasValidData);
    }

    void CreateNew()
    {
        _provider.SetViewModel(new RegistrationViewModel(_provider));
    }

    async Task AuthAsync()
    {
        Error = null;
        var response = await Client.SendAsync(new AuthorizationMessage(Username, Password)).ConfigureAwait(false);
        if(response is JsonMessage json)
        {
            var mainUser = json.GetAs<MainUser>();
            Client.User = mainUser;
            ConfigManager.Save(_context.Config);
            SetViewModel(new HomeViewModel(_provider), true);
            return;
        }

        if(response is ErrorMessage err)
        {
            Error = err;
        }
    }

    bool HasValidData()
    {
        return ValidationHelper.ValidatePassword(Password) && 
               ValidationHelper.ValidateUsername(Username);
    }
}
