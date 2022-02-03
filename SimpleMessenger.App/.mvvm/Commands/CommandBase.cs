using System;
using System.Windows.Input;

namespace SimpleMessenger.App;

abstract class CommandBase : ICommand
{
    public abstract bool CanExecute(object? parameter);
    public abstract void Execute(object? parameter);

    event EventHandler? ICommand.CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}