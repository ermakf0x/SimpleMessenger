using System;
using System.Threading.Tasks;

namespace SimpleMessenger.App;

abstract class BaseViewModel : ObservableObject
{
    protected readonly IViewModelProvider _provider;
    protected Task _initTask;
    public BaseViewModel(IViewModelProvider provider)
        => _provider = provider ?? throw new ArgumentNullException(nameof(provider));

    protected virtual Task InitAsync() { return Task.CompletedTask; }

    protected void SetViewModel(BaseViewModel viewModel, bool withInitialize = false)
    {
        if (withInitialize) _initTask = viewModel.InitAsync();
        else _initTask = Task.CompletedTask;
        _provider.SetViewModel(viewModel);
    }
    protected void ShowModal(BaseModalViewModel baseModalViewModel)
    {
        _provider.ShowModal(baseModalViewModel);
    }
}