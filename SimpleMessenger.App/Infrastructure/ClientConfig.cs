using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimpleMessenger.App.Infrastructure;

class ClientConfig
{
    [JsonConverter(typeof(IPAddressJsonConverter))]
    public IPAddress Address { get; set; }
    public int Port { get; set; }

    public ClientConfig() { }
    public ClientConfig(string addressm, int port)
        : this(IPAddress.Parse(addressm), port) { }
    public ClientConfig(IPAddress address, int port)
    {
        Address = address ?? throw new ArgumentNullException(nameof(address));
        if (port < 0) throw new ArgumentException("Порт не может быть отрицательным числом",nameof(port));
        Port = port;
    }

    public void Save()
    {
        using var fs = File.Open(GetFullPath(), FileMode.Create);
        JsonSerializer.Serialize(fs, this, new JsonSerializerOptions() { WriteIndented = true });
    }
    public static ClientConfig Load(out bool isDefault)
    {
        isDefault = true;
        try
        {
            using var fs = File.OpenRead(GetFullPath());
            var config = JsonSerializer.Deserialize<ClientConfig>(fs);
            if(config is not null)
            {
                isDefault = false;
                return config;
            }
            return CreateDefault();
        }
        catch
        {
            return CreateDefault();
        }
    }
    static ClientConfig CreateDefault()
    {
        return new ClientConfig
        {
            Address = IPAddress.Parse("127.0.0.1"),
            Port = 7777
        };
    }
    static string GetFullPath() => Environment.CurrentDirectory + "\\" + typeof(ClientConfig).Name + ".json";

    class IPAddressJsonConverter : JsonConverter<IPAddress>
    {
        public override IPAddress? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => IPAddress.Parse(reader.GetString());

        public override void Write(Utf8JsonWriter writer, IPAddress value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString());
    }
}
