using SimpleMessenger.Core.Model;

namespace SimpleMessenger.App.Infrastructure;

class UserConfig : IDefaultConfig<UserConfig>
{
    public Token Token { get; set; }
    public int UID { get; set; }

    UserConfig IDefaultConfig<UserConfig>.GetDefault()
    {
        return new UserConfig
        {
            Token = Token.Empty,
            UID = -1
        };
    }
}