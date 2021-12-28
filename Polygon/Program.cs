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
        client.Connect();
        IMessage? msg;

        await client.SendAsync(new TextMessage { Text = "test" });
        await ReciveClientMessageAsync(client);

        await client.SendAsync(new AuthorizationMessage());
        msg = await ReciveClientMessageAsync(client);

        if(msg is AuthSuccessMessage msg2)
        {
            var token = msg2.Token;
            await client.SendAsync(new TextMessage { Text = "text", Token = token });
        }
    }
    static async Task<IMessage?> ReciveClientMessageAsync(SMClient client)
    {
        IMessage? retMsg;
        do
        {
            retMsg = await client.ReciveAsync();
            if (retMsg != null)
            {
                Console.WriteLine($"[CLIENT] {retMsg}");
                return retMsg;
            }
            Thread.Sleep(1);
        } while (retMsg == null);
        return null;
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
