using System.Threading.Tasks;
using System.Windows;

namespace SimpleMessenger.App;

abstract class AsyncCommandBase : CommandBase, IAsyncCommand
{
    public abstract Task ExecuteAsync(object? parameter);
    public override async void Execute(object? parameter)
    {
        try
        {
            await ExecuteAsync(parameter);
        }
        catch (System.Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Command error");
        }
    }
}
