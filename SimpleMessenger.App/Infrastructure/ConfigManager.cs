using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SimpleMessenger.App.Infrastructure;

static class ConfigManager
{
    readonly static Dictionary<Type, IConfig> _cache = new();

    public static T GetOrLoad<T>()
        where T : class, IDefaultConfig<T>, new()
    {
        if (_cache.ContainsKey(typeof(T)))
        {
            return (T)_cache[typeof(T)];
        }

        T? config;
        try
        {
            using var fs = File.OpenRead(GetFullPath<T>());
            config = JsonSerializer.Deserialize<T>(fs);
        }
        catch
        {
            config = new T().GetDefault();
            Save(config, false);
        }

        _cache.Add(typeof(T), config);
        return config;
    }

    public static void Save<T>(T config)
        where T : class, IDefaultConfig<T>, new()
        => Save(config, true);
    public static void SaveAll()
    {
        foreach (var (_, config) in _cache)
            Save(config, false);
    }

    static void Save<T>(T config, bool updateCache)
        where T : IConfig
    {
        return;
        ArgumentNullException.ThrowIfNull(config, nameof(config));

        if (updateCache)
        {
            if (_cache.ContainsKey(typeof(T)))
                _cache[typeof(T)] = config;
            else
                _cache.Add(typeof(T), config);
        }

        using var fs = File.Open(GetFullPath<T>(), FileMode.OpenOrCreate);
        JsonSerializer.Serialize(fs, config, new JsonSerializerOptions() { WriteIndented = true });
    }
    static string GetFullPath<T>() => Environment.CurrentDirectory + "\\" + typeof(T).Name + ".json";
}
