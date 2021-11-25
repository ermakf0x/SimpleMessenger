using System.IO;

namespace SimpleMessenger.Core;

public interface IServerCommand
{
    public CommandType Type { get; }
    public void Write(Stream stream);
    public IServerCommand Read(Stream stream);
}

public enum CommandType : int
{
    Empty,
    Text
}
