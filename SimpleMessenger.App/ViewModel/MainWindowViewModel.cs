using SimpleMessenger.App.Infrastructure;
using SimpleMessenger.App.Infrastructure.Utils;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleMessenger.App.ViewModel;

sealed class MainWindowViewModel : ObservableObject, IViewModelProvider
{
    readonly Stack<BaseViewModel> _vmStack = new();
    BaseViewModel _viewModel;
    BaseModalViewModel _modalViewModel;

    public BaseViewModel ViewModel
    {
        get => _viewModel;
        private set => Set(ref _viewModel, value);
    }
    public BaseModalViewModel ModalViewModel
    {
        get => _modalViewModel;
        private set => Set(ref _modalViewModel, value);
    }

    public MainWindowViewModel()
    {
        _ = InitClientAsync();
    }

    async Task InitClientAsync()
    {
        var config = ClientConfig.Load(out _);
        try
        {
            await Client.ConnectAsync(config);
            ViewModel = new TestViewModel(this);
            return;

            var user = Helper.LoadMainUser();
            if (user is null)
            {
                ((IViewModelProvider)this).SetViewModel(new AuthorizationViewModel(this));
                return;
            }

            Client.User = user;
            var response = await Client.SendAsync(new HelloServerMessage(Client.User.Token));

            ViewModel = response switch
            {
                SuccessMessage => new HomeViewModel(this),
                ErrorMessage err when err.Code == ErrorMessage.Type.TokenInvalid => new AuthorizationViewModel(this),
                ErrorMessage err => new ErrorPageViewModel(this, err.ToString()),
                _ => new ErrorPageViewModel(this, "Что-то пошло не так(((")
            };
        }
        catch (Exception ex)
        {
            ViewModel = new ErrorPageViewModel(this, ex.ToString());
        }
    }

    void IViewModelProvider.SetViewModel(BaseViewModel viewModel)
    {
        ArgumentNullException.ThrowIfNull(viewModel);
        _vmStack.Push(viewModel);
        ViewModel = viewModel;
    }
    void IViewModelProvider.ShowModal(BaseModalViewModel modal)
    {
        ModalViewModel = modal;
    }
    bool IViewModelProvider.Back()
    {
        if (_vmStack.Count == 0) return false;

        BaseViewModel vm;
        do
        {
            vm = _vmStack.Pop();
        } while (ViewModel == vm && _vmStack.Count > 0);

        if (ViewModel == vm) return false;
        _vmStack.Push(vm);
        ViewModel = vm;
        return true;
    }
}

class TestViewModel : BaseViewModel
{
    public ICommand TestCommand { get; set; }
    public ICommand Test2Command { get; set; }
    public TestViewModel(IViewModelProvider provider) : base(provider)
    {
        TestCommand = new AsyncCommand(TestAsync);
        Test2Command = new AsyncCommand(Test2Async);
    }

    private Task TestAsync() => AuthAsync("User", "qwerty1234");
    private Task Test2Async() => AuthAsync("User2", "qwerty1234");

    async Task AuthAsync(string username, string password)
    {
        var response = await Client.SendAsync(new AuthorizationMessage(username, password)).ConfigureAwait(false);

        if (response is JsonMessage json)
        {
            var mainUser = json.GetAs<MainUser>();
            Client.User = mainUser;
            Helper.SaveMainUser(mainUser);
            SetViewModel(new HomeViewModel(_provider), true);
            return;
        }

        if (response is ErrorMessage err)
        {
            SetViewModel(new ErrorPageViewModel(_provider, err.Message));
        }
    }
}