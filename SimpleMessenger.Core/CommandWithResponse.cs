using System.Threading;
using System.Threading.Tasks;

namespace SimpleMessenger.Core;

public class CommandWithResponse : Command
{
    public IMessage Response { get; protected set; }
    public CommandWithResponse(IMessage message) : base(message) { }
    public override async Task ExecuteAsync(NetworkChannel channel)
    {
        await channel.SendAsync(Message);
        while (true)
        {
            if(!channel.MessageAvailable)
            {
                Thread.Sleep(1);
                continue;
            }
            Response = await channel.ReceiveAsync();
            return;
        }
    }
}
