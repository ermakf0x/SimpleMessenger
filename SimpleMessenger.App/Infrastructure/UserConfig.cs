using SimpleMessenger.Core.Model;

namespace SimpleMessenger.App.Infrastructure;

class UserConfig : MainUser, IConfig<UserConfig>
{
    public UserConfig() { }
    public UserConfig(MainUser user)
    {
        UID = user.UID;
        Token = user.Token;
        Name = user.Name;
        Username = user.Username;
    }
    UserConfig IConfig<UserConfig>.GetDefault()
    {
        return new UserConfig
        {
            UID = 0,
            Token = Token.Empty,
            Name = "Default",
            Username = "Default",
        };
    }
}