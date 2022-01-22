using SimpleMessenger.App.Infrastructure;
using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using System;
using System.Collections.Generic;

namespace SimpleMessenger.App.ViewModel;

sealed class MainWindowViewModel : ObservableObject, IViewModelProvider
{
    readonly Stack<BaseViewModel> _vmStack = new();
    readonly SMClient _client;
    BaseViewModel _viewModel;

    public BaseViewModel ViewModel
    {
        get => _viewModel;
        private set => Set(ref _viewModel, value);
    }

    public MainWindowViewModel()
    {
        var config = ConfigManager.GetOrLoad<SMClientConfig>();
        _client = new SMClient(config.IPAddres, config.Port);
        Global.Client = _client;

        InitSMClient(ConfigManager.GetOrLoad<UserConfig>());

        //var context = new ClientContext { Config = ConfigManager.GetOrLoad<UserConfig>(), Client = _client };
        //var vm = new AuthorizationViewModel(this, context);
        //((IViewModelProvider)this).ChangeViewModel(vm);
    }
    ~MainWindowViewModel()
    {
        Console.WriteLine("Destroy this class");
    }

    async void InitSMClient(UserConfig config)
    {
        try
        {
            var response = await _client.ConnectAsync(new HelloServerMessage(config.Token));
            var context = new ClientContext
            {
                Config = config,
                Client = _client
            };

            ViewModel = response switch
            {
                SuccessMessage => new HomeViewModel(this, context),
                ErrorMessage err when err.Code == ErrorMessage.Type.TokenInvalid => new AuthorizationViewModel(this, context),
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