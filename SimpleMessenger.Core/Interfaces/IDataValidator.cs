namespace SimpleMessenger.Core;

public interface IDataValidator<T>
{
    bool HasValid(T data);
}