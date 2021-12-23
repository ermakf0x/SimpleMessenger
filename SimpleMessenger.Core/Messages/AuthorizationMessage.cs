using System;
using System.IO;

namespace SimpleMessenger.Core
{
    public class AuthorizationMessage : IMessage
    {
        public MessageType Type => MessageType.Authorization;

        //public AuthorizationMessage() { }
        public AuthorizationMessage()
        {

        }

        public void Read(Stream stream)
        {
            throw new NotImplementedException();
        }

        public void Write(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
