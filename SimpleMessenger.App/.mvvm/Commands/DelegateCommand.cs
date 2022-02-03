using System;

namespace SimpleMessenger.App;

class DelegateCommand : CommandBase
{
    readonly Action _execute;
    readonly Func<bool>? _canExecute;

    public DelegateCommand(Action execute) : this(execute, null) { }
    public DelegateCommand(Action execute, Func<bool>? canExecute)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public override bool CanExecute(object? parameter) => _canExecute is null || _canExecute();
    public override void Execute(object? parameter) => _execute();
}