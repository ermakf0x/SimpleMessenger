using SimpleMessenger.Core.Model;

namespace SimpleMessenger.App.Infrastructure.Utils;

static class Helper
{
    public static void SaveMainUser(MainUser user)
    {
        JsonStorage.Save(user);
    }
    public static MainUser? LoadMainUser()
    {
        return JsonStorage.Load<MainUser>();
    }
}