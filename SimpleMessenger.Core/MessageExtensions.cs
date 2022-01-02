namespace SimpleMessenger.Core;

public static class MessageExtensions
{
    public static Command AsCommand(this IMessage message) => new(message);
}
