using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Core.Messages;

public interface ITokenable : IMessage
{
    Token Token { get; }
}
