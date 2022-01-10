using System.Collections.Generic;
using System.IO;

namespace SimpleMessenger.Core.Messages;

public class ResponseUsersMessage : IMessage
{
    public MessageType Type => MessageType.ResponseUsers;
    public List<UserData> Users { get; set; }

    internal ResponseUsersMessage() { }
    public ResponseUsersMessage(List<UserData> users) => Users = users;

    public void Read(Stream stream)
    {
        var usersCount = stream.Read<int>();
        if (usersCount > 0)
        {
            Users = new List<UserData>(usersCount);
            for (int i = 0; i < usersCount; i++)
            {
                Users.Add(new()
                {
                    Id = stream.Read<int>(),
                    Name = stream.ReadString()
                });
            }
        }
    }

    public void Write(Stream stream)
    {
        if (Users != null && Users.Count > 0)
        {
            stream.Write(Users.Count);
            foreach (var user in Users)
            {
                stream.Write(user.Id);
                stream.Write(user.Name);
            }
        }
        else stream.Write(0);
    }
}
