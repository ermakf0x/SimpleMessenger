using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server;

static class ErrorMsgHelper
{
    public static ErrorMessage NotAuthorized => new("Пользователь не авторизован.", ErrorMessage.Type.NotAuthorized);
}
