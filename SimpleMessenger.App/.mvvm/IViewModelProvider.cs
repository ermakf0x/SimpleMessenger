namespace SimpleMessenger.App;

interface IViewModelProvider
{
    BaseViewModel ViewModel { get; }
    bool ChangeViewModel(BaseViewModel vm);
    bool Back();
}
