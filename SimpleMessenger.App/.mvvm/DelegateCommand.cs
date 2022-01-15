using System;

namespace SimpleMessenger.App;

class DelegateCommand : CommandBase
{
    readonly Action _execute;

    public DelegateCommand(Action execute)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
    }

    public override bool CanExecute(object? parameter) => true;
    public override void Execute(object? parameter) => _execute();
}