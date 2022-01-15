using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleMessenger.App;

interface IAsyncCommand : ICommand
{
    Task ExecuteAsync(object? parameter);
}