using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Server;

class Program
{
    static void Main(string[] args)
    {
        StartTestServer();
        Thread.Sleep(100);
        StartTestClient();

        Console.ReadLine();
        //while (true)
        //{
        //    var message = Console.ReadLine();
        //    Console.Clear();
        //    if (string.IsNullOrWhiteSpace(message)) continue;
        //    client.Send(new TextMessage(message));
        //}
    }

    static async void StartTestClient()
    {
        var client = new SMClient("127.0.0.1", 7777);

        var res = await client.SendAsync(new TextMessage(default, "test"));
        PrintMessage(res);

        res = await client.SendAsync(new AuthorizationMessage("user3"));
        PrintMessage(res);

        if(res.Success())
        {
            var jContent = res as JsonContent;
            var token = (Token)jContent.Data;
            await client.SendAsync(new TextMessage(token, "text"));
        }

        void PrintMessage(IMessage message)
        {
            Console.WriteLine($"[CLIENT]: {message}");
        }
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
