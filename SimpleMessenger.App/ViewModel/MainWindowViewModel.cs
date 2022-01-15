namespace SimpleMessenger.App.ViewModel;

sealed class MainWindowViewModel : ObservableObject, IViewModelProvider
{
    ViewModelBase _viewModel;

    public ViewModelBase ViewModel
    {
        get => _viewModel;
        private set => Set(ref _viewModel, value);
    }

    public MainWindowViewModel()
    {
        ViewModel = new RegistrationViewModel(this);
    }

    bool IViewModelProvider.ChangeViewModel(ViewModelBase vm)
    {
        _viewModel?.OnChangedViewModel();
        ViewModel = vm;
        return true;
    }
}