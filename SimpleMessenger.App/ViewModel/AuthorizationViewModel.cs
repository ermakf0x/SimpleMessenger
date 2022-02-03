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
    readonly ClientContext _context;
    ErrorMessage? _error;

    public string Username { get; set; }
    public string Password { get; set; }
    public ErrorMessage? Error { get => _error; set => Set(ref _error, value); }

    public ICommand CreateNewCommand { get; }
    public IAsyncCommand AuthCommand { get; }

    public AuthorizationViewModel(IViewModelProvider provider, ClientContext context) : base(provider)
    {
        ArgumentNullException.ThrowIfNull(nameof(context));
        _context = context;

        CreateNewCommand = new DelegateCommand(CreateNew);
        AuthCommand = new AsyncCommand(AuthAsync, HasValidData);
    }

    void CreateNew()
    {
        _provider.SetViewModel(new RegistrationViewModel(_provider, _context));
    }

    async Task AuthAsync()
    {
        Error = null;
        var response = await _context.Server.SendAsync(new AuthorizationMessage(Username, Password)).ConfigureAwait(false);
        if(response is JsonMessage json)
        {
            var mainUser = json.GetAs<MainUser>();
            _context.Config = new UserConfig(mainUser);
            ConfigManager.Save(_context.Config);
            using var ls = new LocalStorage();
            await ls.InitAsync(_context.Config).ConfigureAwait(false);
            _provider.SetViewModel(new HomeViewModel(_provider, _context));
            return;
        }

        if(response is ErrorMessage err)
        {
            Error = err;
        }
    }

    bool HasValidData()
    {
        return DataValidator.PasswordValidator.HasValid(Password) && 
               DataValidator.UsernameValidator.HasValid(Username);
    }
}
