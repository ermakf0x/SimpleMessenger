using SimpleMessenger.Core.Model;

namespace SimpleMessenger.Core;

public interface ITokenable : IMessage
{
    Token Token { get; }
}
