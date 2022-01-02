using System;
using System.Threading.Tasks;

namespace SimpleMessenger.Core;

public class Command
{
    public IMessage Message { get; }
    public virtual Task ExecuteAsync(NetworkChannel channel) => channel.SendAsync(Message);
    public Command(IMessage message)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
    }
}
