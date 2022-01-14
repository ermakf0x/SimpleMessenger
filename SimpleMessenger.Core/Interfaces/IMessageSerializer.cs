namespace SimpleMessenger.Core;

public interface IMessageSerializer
{
    void Serialize(Stream stream, IMessage message);
    IMessage Desirialize(Stream stream);
}