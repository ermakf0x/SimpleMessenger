using System;

namespace SimpleMessenger.App;

abstract class BaseViewModel : ObservableObject
{
    protected readonly IViewModelProvider _provider;
    public BaseViewModel(IViewModelProvider provider)
        => _provider = provider ?? throw new ArgumentNullException(nameof(provider));

    protected void SetViewModel(BaseViewModel viewModel)
    {
        _provider.SetViewModel(viewModel);
    }
    protected void ShowModal(BaseModalViewModel baseModalViewModel)
    {
        _provider.ShowModal(baseModalViewModel);
    }
}