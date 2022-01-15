namespace SimpleMessenger.App;

interface IViewModelProvider
{
    ViewModelBase ViewModel { get; }
    bool ChangeViewModel(ViewModelBase vm);
}
