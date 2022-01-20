using System;
using System.Threading.Tasks;

namespace SimpleMessenger.App;

class AsyncCommand : AsyncCommandBase
{
    readonly Func<Task> _execute;
    readonly Func<bool>? _canExecute;

    public AsyncCommand(Func<Task> execute, Func<bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public override bool CanExecute(object? parameter) => _canExecute is null || _canExecute();
    public override Task ExecuteAsync(object? parameter) => _execute();
}