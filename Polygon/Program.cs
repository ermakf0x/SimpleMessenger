using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Server;

class Program
{
    static void Main(string[] args)
    {
        StartTestServer();
        Thread.Sleep(100);
        var client = new SMClient("127.0.0.1", 7777);
        client.Connect();

        client.SendAsync(new TextMessage("test"));
        client.SendAsync(new TextMessage("test234"));
        client.SendAsync(new TextMessage("test2sdfsdfsf34"));
        Console.ReadLine();
        //while (true)
        //{
        //    var message = Console.ReadLine();
        //    Console.Clear();
        //    if (string.IsNullOrWhiteSpace(message)) continue;
        //    client.Send(new TextMessage(message));
        //}
    }

    static void StartTestServer()
    {
        new Thread(new ThreadStart(() =>
        {
            var server = new Server();
            server.Start();
        })).Start();
    }
}
