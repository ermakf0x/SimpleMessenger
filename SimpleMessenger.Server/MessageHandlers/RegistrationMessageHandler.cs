﻿using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;

namespace SimpleMessenger.Server.MessageHandlers;
class RegistrationMessageHandler : ServerMessageHandlerBase<RegistrationMessage>
{
    protected override IResponse Process(RegistrationMessage message, ServerClient client)
    {
        try
        {
            var newUser = LocalDb.New(message.Login, message.Password, message.Name);
            client.User = newUser;
            return JContent(newUser.ToClientUser());
        }
        catch (Exception)
        {
            return Error("");
        }
    }
}
