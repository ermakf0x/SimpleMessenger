using SimpleMessenger.App.Infrastructure;
using SimpleMessenger.Core.Messages;
using System;
using System.Collections.Generic;

namespace SimpleMessenger.App.ViewModel;

sealed class MainWindowViewModel : ObservableObject, IViewModelProvider
{
    readonly Stack<BaseViewModel> _vmStack = new();
    readonly ClientContext _context;
    BaseViewModel _viewModel;

    public BaseViewModel ViewModel
    {
        get => _viewModel;
        private set => Set(ref _viewModel, value);
    }

    public MainWindowViewModel()
    {
        var config = ConfigManager.Load<LocalServerConfig>();
        _context = new ClientContext
        {
            Server = new LocalServer(config),
            Config = ConfigManager.Load<UserConfig>(),
        };
        InitSMClient();
    }

    async void InitSMClient()
    {
        try
        {
            await _context.Server.ConnectToServerAsync();
            var response = await _context.Server.SendAsync(new HelloServerMessage(_context.Config.Token));

            ViewModel = response switch
            {
                SuccessMessage => new HomeViewModel(this, _context),
                ErrorMessage err when err.Code == ErrorMessage.Type.TokenInvalid => new AuthorizationViewModel(this, _context),
                ErrorMessage err => new ErrorPageViewModel(this, err.ToString()),
                _ => new ErrorPageViewModel(this, "Что-то пошло не так(((")
            };
        }
        catch (Exception ex)
        {
            ViewModel = new ErrorPageViewModel(this, ex.ToString());
        }
    }

    bool IViewModelProvider.ChangeViewModel(BaseViewModel vm)
    {
        ArgumentNullException.ThrowIfNull(vm);
        _vmStack.Push(vm);
        ViewModel?.OnChangedViewModel();
        ViewModel = vm;
        return true;
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