using System;

namespace SimpleMessenger.App;

abstract class BaseViewModel : ObservableObject
{
    protected readonly IViewModelProvider _provider;
    public BaseViewModel(IViewModelProvider provider)
        => _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    public virtual void OnChangedViewModel() { }
}
