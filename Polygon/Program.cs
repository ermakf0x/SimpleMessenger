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
        IMessage? msg;

        var cmd = new CommandWithResponse(new TextMessage(default, "test"));
        await client.SendAsync(cmd);
        PrintMessage(cmd.Response);

        cmd = new CommandWithResponse(new AuthorizationMessage("user3"));
        await client.SendAsync(cmd);
        PrintMessage(cmd.Response);
        msg = cmd.Response;

        if(msg is AuthSuccessMessage msg2)
        {
            var token = msg2.Token;
            await client.SendAsync(new TextMessage(token, "text").AsCommand());
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
