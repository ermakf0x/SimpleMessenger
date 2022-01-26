namespace SimpleMessenger.Server;

static class Program
{
    static void Main(string[] args)
    {
        Logger.Initialize();
        try
        {
            Server.Instance.Run();
        }
        catch (Exception e)
        {
            Logger.Fatal(e);
        }
    }
}
