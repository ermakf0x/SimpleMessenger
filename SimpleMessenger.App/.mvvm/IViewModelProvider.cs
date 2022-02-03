namespace SimpleMessenger.App;

interface IViewModelProvider
{
    BaseViewModel ViewModel { get; }
    BaseModalViewModel ModalViewModel { get; }
    void SetViewModel(BaseViewModel viewModel);
    void ShowModal(BaseModalViewModel modal);
    bool Back();
}