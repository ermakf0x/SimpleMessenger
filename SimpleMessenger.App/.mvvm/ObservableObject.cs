using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SimpleMessenger.App;

abstract class ObservableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual bool Set<T>(ref T property, T value, [CallerMemberName] string? propertyName = null)
    {
        if (property is null || !property.Equals(value))
        {
            property = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        return false;
    }
}