using SimpleMessenger.Core;

namespace SimpleMessenger.App.Infrastructure;

sealed class ClientContext
{
    public UserConfig Config { get; init; }
    public SMClient Client { get; init; }
}
