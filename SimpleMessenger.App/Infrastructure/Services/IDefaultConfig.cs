namespace SimpleMessenger.App.Infrastructure.Services;

interface IConfig { }
interface IDefaultConfig<T> : IConfig
    where T : class, new()
{
    T GetDefault();
}