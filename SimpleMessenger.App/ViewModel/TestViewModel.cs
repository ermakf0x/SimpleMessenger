namespace SimpleMessenger.App.ViewModel;

class TestViewModel : BaseViewModel
{
    public string Name { get; set; } = "TestViewModel";
    public TestViewModel(IViewModelProvider provider) : base(provider) { }
}
