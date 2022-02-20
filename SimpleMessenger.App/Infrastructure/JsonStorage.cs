using System;
using System.IO;
using System.Text.Json;

namespace SimpleMessenger.App.Infrastructure;

static class JsonStorage
{
    public static bool Save<T>(T obj)
    {
        try
        {
            using var fs = File.Open(GetFullPathByType<T>(), FileMode.Create);
            JsonSerializer.Serialize(fs, obj, new JsonSerializerOptions() { WriteIndented = true });
            return true;
        }
        catch
        {
            return false;
        }
    }
    public static T? Load<T>()
    {
        try
        {
            using var fs = File.OpenRead(GetFullPathByType<T>());
            return JsonSerializer.Deserialize<T>(fs);
        }
        catch
        {
            return default;
        }
    }
    public static string GetFullPathByType<T>() => Environment.CurrentDirectory + "\\" + typeof(T).Name + ".json";
}