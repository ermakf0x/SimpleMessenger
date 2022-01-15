using System;
using System.Threading.Tasks;

namespace SimpleMessenger.App;

class AsyncCommand : AsyncCommandBase
{
    readonly Func<Task> _execute;

    public AsyncCommand(Func<Task> execute)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
    }

    public override bool CanExecute(object? parameter) => true;
    public override Task ExecuteAsync(object? parameter) => _execute();
}