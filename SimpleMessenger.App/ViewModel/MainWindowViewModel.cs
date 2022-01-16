using SimpleMessenger.App.Infrastructure.Services;
using SimpleMessenger.App.Model;
using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using System;

namespace SimpleMessenger.App.ViewModel;

sealed class MainWindowViewModel : ObservableObject, IViewModelProvider
{
    readonly SMClient _client;
    ViewModelBase _viewModel;

    public ViewModelBase ViewModel
    {
        get => _viewModel;
        private set => Set(ref _viewModel, value);
    }

    public MainWindowViewModel()
    {
        var config = ConfigManager.GetOrLoad<SMClientConfig>();
        _client = new SMClient(config.IPAddres, config.Port);
        InitSMClient(ConfigManager.GetOrLoad<UserConfig>());
    }

    async void InitSMClient(UserConfig config)
    {
        try
        {
            var response = await _client.ConnectAsync(new HelloServerMessage(config.Token));
            ViewModel = response switch
            {
                SuccessMessage => new HomeViewModel(this, _client),
                ErrorMessage err => new ErrorPageViewModel(this, err.ToString()),
                _ => new ErrorPageViewModel(this, "Чтото пошло не так(((")
            };
        }
        catch (Exception ex)
        {
            ViewModel = new ErrorPageViewModel(this, ex.ToString());
        }
    }

    bool IViewModelProvider.ChangeViewModel(ViewModelBase vm)
    {
        ViewModel?.OnChangedViewModel();
        ViewModel = vm;
        return true;
    }
}