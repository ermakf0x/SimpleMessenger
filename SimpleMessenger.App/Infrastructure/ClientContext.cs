namespace SimpleMessenger.App.Infrastructure;

sealed class ClientContext
{
    public UserConfig Config { get; set; }
    public LocalServer Server { get; init; }
}
