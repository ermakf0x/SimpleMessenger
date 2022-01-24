using System;
using System.IO;
using System.Text.Json;

namespace SimpleMessenger.App.Infrastructure;

interface IConfig<T> where T : class, new()
{
    T GetDefault();
}

static class ConfigManager
{
    public static T Load<T>()
        where T : class, IConfig<T>, new()
    {
        T? config = null;
        try
        {
            using var fs = File.OpenRead(GetFullPath<T>());
            config = JsonSerializer.Deserialize<T>(fs);
        }
        catch { }

        if(config == null)
        {
            config = new T().GetDefault();
            Save(config);
        }

        return config;
    }

    public static void Save<T>(T config)
        where T : class, IConfig<T>, new()
    {
        ArgumentNullException.ThrowIfNull(config, nameof(config));
        using var fs = File.Open(GetFullPath<T>(), FileMode.Create);
        JsonSerializer.Serialize(fs, config, new JsonSerializerOptions() { WriteIndented = true });
    }

    static string GetFullPath<T>() => Environment.CurrentDirectory + "\\" + typeof(T).Name + ".json";
}
