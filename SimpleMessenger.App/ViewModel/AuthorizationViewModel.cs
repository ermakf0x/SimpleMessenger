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
    readonly SMClient _client;
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
        _client = context.Client;

        CreateNewCommand = new DelegateCommand(CreateNew);
        AuthCommand = new AsyncCommand(AuthAsync, HasValidData);
    }

    void CreateNew()
    {
        _provider.ChangeViewModel(new RegistrationViewModel(_provider, _context));
    }

    async Task AuthAsync()
    {
        Error = null;
        var response = await _client.SendAsync(new AuthorizationMessage(Username, Password));
        if(response is JsonMessage json)
        {
            _context.Config.Token = json.GetAs<Token>();
            ConfigManager.Save(_context.Config);
            _provider.ChangeViewModel(new HomeViewModel(_provider, _context));
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
