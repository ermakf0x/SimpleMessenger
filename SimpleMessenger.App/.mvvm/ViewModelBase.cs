using System;

namespace SimpleMessenger.App;

abstract class ViewModelBase : ObservableObject
{
    protected readonly IViewModelProvider _provider;
    public ViewModelBase(IViewModelProvider provider)
        => _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    public virtual void OnChangedViewModel() { }
}
