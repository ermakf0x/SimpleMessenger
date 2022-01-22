namespace SimpleMessenger.App.ViewModel;

class ErrorPageViewModel : BaseViewModel
{
    string _error;
    public string ErrorMessage { get => _error; set => Set(ref _error, value); }

    public ErrorPageViewModel(IViewModelProvider provider, string error) : base(provider)
    {
        ErrorMessage = error;
    }
}
