using SimpleMessenger.Core;

class Program
{
    static void Main(string[] args)
    {
        StartTestServer();
        Thread.Sleep(100);
        var client = new SMClient("127.0.0.1", 7777);
        client.Connect();

        client.Send(new TextMessage("test"));
        client.Send(new TextMessage("test2"));
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
            var server = new SMServer();
            server.Start();
        })).Start();
    }
}
