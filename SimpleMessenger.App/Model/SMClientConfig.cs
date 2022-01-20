using SimpleMessenger.App.Infrastructure;

namespace SimpleMessenger.App.Model;

class SMClientConfig : IDefaultConfig<SMClientConfig>
{
    public string IPAddres { get; set; }
    public int Port { get; set; }

    SMClientConfig IDefaultConfig<SMClientConfig>.GetDefault()
    {
        return new()
        {
            IPAddres = "127.0.0.1",
            Port = 7777
        };
    }
}