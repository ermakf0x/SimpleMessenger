using System;

namespace SimpleMessenger.App.Infrastructure.Utils;

sealed class Instance<T> where T : class
{
    T? _instance;

    public void Set(T instance, bool disposeLastInstance = false)
    {
        if (disposeLastInstance && _instance is not null && _instance is IDisposable inst)
        {
            inst.Dispose();
        }
        _instance = instance;
    }
    public T Get()
    {
        Check();
        return _instance;
    }
    public void Check()
    {
        if (_instance == null) throw new InvalidOperationException("");
    }
    public static explicit operator T?(Instance<T> instance) => instance._instance;
    public override string ToString() => _instance?.ToString() ?? "null";
}
