using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SimpleMessenger.Core.Messages;

public class ResponseUsersMessage : IMessage
{
    public MessageType Type => MessageType.ResponseUsers;
    public List<UserData> Users { get; set; }

    public ResponseUsersMessage() { }
    public ResponseUsersMessage(List<UserData> users) => Users = users;

    public void Read(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[1024];
        stream.Read(buffer[..4]); // Количество пользователей в потоке
        var countUsers = BitConverter.ToInt32(buffer[..4]);
        var blockSize = 0;

        if (countUsers <= 0) return;
        Users = new List<UserData>(countUsers);
        for (int i = 0; i < countUsers; i++)
        {
            stream.Read(buffer[..4]); // Размер блока с данными
            blockSize = BitConverter.ToInt32(buffer);
            if (blockSize > 5)
            {
                var block = buffer[..blockSize];
                stream.Read(block);
                Users.Add(ReadUser(block));
            }
        }
    }

    public void Write(Stream stream)
    {
        if(Users != null && Users.Count > 0)
        {
            Span<byte> buffer = stackalloc byte[1024];
            var count = 0;

            stream.Write(BitConverter.GetBytes(Users.Count));

            foreach (var user in Users)
            {
                count = WriteUser(buffer, user);
                stream.Write(buffer[..count]);
            }
        }
    }

    int WriteUser(Span<byte> buffer, UserData user)
    {
        BitConverter.TryWriteBytes(buffer[4..], user.Id);
        var sizeStr = Encoding.UTF8.GetBytes(user.Name, buffer[8..]);
        BitConverter.TryWriteBytes(buffer, sizeStr + 4);
        return sizeStr + 8;
    }
    UserData ReadUser(Span<byte> buffer)
    {
        return new UserData
        {
            Id = BitConverter.ToInt32(buffer),
            Name = Encoding.UTF8.GetString(buffer[4..])
        };
    }
}
