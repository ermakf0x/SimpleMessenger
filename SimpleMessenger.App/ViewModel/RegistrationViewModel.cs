using SimpleMessenger.App.Infrastructure;
using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleMessenger.App.ViewModel;

class RegistrationViewModel : BaseViewModel
{
    readonly ClientContext _context;
    ErrorMessage? _error;

    public string Username { get; set; }
    public string Password { get; set; }
    public string Password2 { get; set; }
    public ErrorMessage? Error { get => _error; set => Set(ref _error, value); }

    public ICommand BackCommand { get; }
    public ICommand CreateCommand { get; }

    public RegistrationViewModel(IViewModelProvider provider, ClientContext context) : base(provider)
    {
        ArgumentNullException.ThrowIfNull(nameof(context));
        _context = context;

        BackCommand = new DelegateCommand(() => _provider.Back());
        CreateCommand = new AsyncCommand(CreateAsync, CanCreated);
    }

    async Task CreateAsync()
    {
        Error = null;
        var responce = await _context.Server.SendAsync(new RegistrationMessage(Username, Password, Username)).ConfigureAwait(false);

        if(responce is JsonMessage json)
        {
            var user = json.GetAs<MainUser>();
            _context.Config.Token = user.Token;
            ConfigManager.Save(_context.Config);
            using var ls = new LocalStorage();
            await ls.InitAsync(_context.Config).ConfigureAwait(false);
            _provider.SetViewModel(new HomeViewModel(_provider, _context));
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
