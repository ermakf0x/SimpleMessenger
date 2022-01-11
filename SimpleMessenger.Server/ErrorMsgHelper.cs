using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server;

static class ErrorMsgHelper
{
    public static Error NotAuthorized => new("Пользователь не авторизован.", ErrorType.NotAuthorized);
}
