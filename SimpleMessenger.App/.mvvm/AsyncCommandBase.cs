using System.Threading.Tasks;

namespace SimpleMessenger.App;

abstract class AsyncCommandBase : CommandBase, IAsyncCommand
{
    public abstract Task ExecuteAsync(object? parameter);
    public override async void Execute(object? parameter) => await ExecuteAsync(parameter);
}
