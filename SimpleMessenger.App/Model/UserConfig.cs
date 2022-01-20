using SimpleMessenger.App.Infrastructure;
using SimpleMessenger.Core;
using SimpleMessenger.Core.Model;

namespace SimpleMessenger.App.Model;

class UserConfig : IDefaultConfig<UserConfig>
{
    public Token Token { get; set; }

    UserConfig IDefaultConfig<UserConfig>.GetDefault()
    {
        return new UserConfig
        {
            Token = Token.Empty
        };
    }
}