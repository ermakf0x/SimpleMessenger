using System;
using System.Windows.Input;

namespace SimpleMessenger.App;

abstract class BaseModalViewModel : ObservableObject
{
    protected readonly IViewModelProvider _provider;
    public ICommand CloseModalCommand { get; }

    public BaseModalViewModel(IViewModelProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        CloseModalCommand = new DelegateCommand(Close);
    }

    protected void ShowModal(BaseModalViewModel modal)
    {
        _provider.ShowModal(modal);
    }
    protected void Close()
    {
        _provider.ShowModal(null);
    }
}