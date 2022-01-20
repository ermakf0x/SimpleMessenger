using SimpleMessenger.Core;

namespace SimpleMessenger.App.Model;

sealed class ClientContext
{
    public UserConfig Config { get; init; }
    public SMClient Client { get; init; }
}
