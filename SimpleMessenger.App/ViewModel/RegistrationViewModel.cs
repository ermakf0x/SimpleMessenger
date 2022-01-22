using SimpleMessenger.App.Infrastructure;
using SimpleMessenger.App.Model;
using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleMessenger.App.ViewModel;

class RegistrationViewModel : BaseViewModel
{
    readonly SMClient _client;
    readonly ClientContext _context;
    ErrorMessage? _error;

    public string Username { get; set; }
    public string Password { get; set; }
    public string Password2 { get; set; }
    public ErrorMessage? Error { get => _error; set => Set(ref _error, value); }

    public ICommand BackCommand { get; }
    public IAsyncCommand CreateCommand { get; }

    public RegistrationViewModel(IViewModelProvider provider, ClientContext context) : base(provider)
    {
        ArgumentNullException.ThrowIfNull(nameof(context));
        _context = context;
        _client = context.Client;

        BackCommand = new DelegateCommand(() => _provider.Back());
        CreateCommand = new AsyncCommand(CreateAsync, CanCreated);
    }

    async Task CreateAsync()
    {
        Error = null;
        var responce = await _client.SendAsync(new RegistrationMessage(Username, Password, Username));

        if(responce is JsonMessage json)
        {
            var user = json.GetAs<MainUser>();
            _context.Config.Token = user.Token;
            ConfigManager.Save(_context.Config);
            _provider.ChangeViewModel(new HomeViewModel(_provider, _context));
            return;
        }

        if(responce is ErrorMessage err)
        {
            Error = err;
        }
    }

    bool CanCreated()
    {
        return DataValidator.PasswordValidator.HasValid(Password) &&
               DataValidator.UsernameValidator.HasValid(Username) &&
               Password == Password2;
    }
}
