using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Core;

public static class ResponseMessageExtensions
{
    public static bool Success(this IResponse response) => response is Error;
}
