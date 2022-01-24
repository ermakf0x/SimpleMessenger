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
    }


    UserConfig IConfig<UserConfig>.GetDefault()
    {
        return new UserConfig
        {
            UID = 0,
            Name = "Default",
            Token = Token.Empty
        };
    }
}