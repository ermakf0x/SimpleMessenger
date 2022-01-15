namespace SimpleMessenger.App.ViewModel;

class TestViewModel : ViewModelBase
{
    public string Name { get; set; } = "TestViewModel";
    public TestViewModel(IViewModelProvider provider) : base(provider) { }
}
