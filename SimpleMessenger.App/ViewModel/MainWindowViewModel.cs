﻿using SimpleMessenger.App.Infrastructure;
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
    readonly ClientContext _context;
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
            ViewModel = new TestViewModel(this, _context);
            return;


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
    readonly ClientContext context;
    public ICommand TestCommand { get; set; }
    public ICommand Test2Command { get; set; }
    public TestViewModel(IViewModelProvider provider, ClientContext context) : base(provider)
    {
        this.context = context;

        TestCommand = new AsyncCommand(TestAsync);
        Test2Command = new AsyncCommand(Test2Async);
    }

    private Task TestAsync() => AuthAsync("User", "qwerty1234");
    private Task Test2Async() => AuthAsync("User2", "qwerty1234");

    async Task AuthAsync(string username, string password)
    {
        var response = await context.Server.SendAsync(new AuthorizationMessage(username, password)).ConfigureAwait(false);

        if (response is JsonMessage json)
        {
            var mainUser = json.GetAs<MainUser>();
            context.Config = new UserConfig(mainUser);
            ConfigManager.Save(context.Config);
            using var ls = new LocalStorage();
            await ls.InitAsync(context.Config).ConfigureAwait(false);
            SetViewModel(new HomeViewModel(_provider, context));
            return;
        }

        if (response is ErrorMessage err)
        {
            SetViewModel(new ErrorPageViewModel(_provider, err.Message));
        }
    }
}