namespace SimpleMessenger.App.Infrastructure;

interface IConfig { }
interface IDefaultConfig<T> : IConfig
    where T : class, new()
{
    T GetDefault();
}