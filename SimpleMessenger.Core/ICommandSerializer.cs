using System.IO;

namespace SimpleMessenger.Core;

public interface ICommandSerializer
{
    IServerCommand Desirialize(Stream stream);
    void Serialize(Stream stream, IServerCommand command);
}