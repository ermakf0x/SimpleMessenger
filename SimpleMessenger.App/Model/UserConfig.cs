﻿using SimpleMessenger.App.Infrastructure.Services;
using SimpleMessenger.Core;

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