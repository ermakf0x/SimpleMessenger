using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;

namespace SimpleMessenger.Core;

public class CommandSerializer : ICommandSerializer
{
    const int SIZE = sizeof(long);
    static readonly byte[] buffer = new byte[SIZE];
    static readonly Dictionary<CommandType, Type> table = new()
    {
        { CommandType.Text, typeof(TextMessage) }
    };

    public void Serialize(Stream stream, IServerCommand command)
    {
        using var ms = new MemoryStream(256);
        ms.Seek(sizeof(long), SeekOrigin.Begin);
        ms.Write(BitConverter.GetBytes((int)command.Type));
        command.Write(ms);
        ms.Position = 0;
        ms.Write(BitConverter.GetBytes(ms.Length));
        ms.Position = ms.Length;
        ms.WriteTo(stream);
    }

    public IServerCommand Desirialize(Stream stream)
    {
        stream.Read(buffer);
        var sizeBlock = BitConverter.ToInt32(buffer, 0) - SIZE;
        using var memory = MemoryPool<byte>.Shared.Rent(sizeBlock);
        var buf = memory.Memory.Span[..sizeBlock];
        var count = stream.Read(buf);

        var type = (CommandType)BitConverter.ToInt32(buf);
        var command = CreateCommandFromType(type);
        using var ms = new MemoryStream(buf[4..].ToArray()); // Вычитывает 4 bit которые шли как тип команды
        command.Read(ms);
        return command;
    }

    static IServerCommand CreateCommandFromType(CommandType type)
    {
        return (IServerCommand)Activator.CreateInstance(table[type]);
    }
}
