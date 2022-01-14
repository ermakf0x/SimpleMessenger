namespace SimpleMessenger.Core.Messages;

public class ErrorMessage : IResponse
{
    public MessageType MessageType => MessageType.Error;
    public Type Code { get; protected set; }
    public string Message { get; protected set; }

    internal ErrorMessage() { }
    public ErrorMessage(string message, Type code = Type.Other)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Code = code;
    }

    public void Read(Stream stream)
    {
        Code = stream.Read<Type>();
        Message = stream.ReadString();
    }

    public void Write(Stream stream)
    {
        stream.Write(Code);
        stream.Write(Message);
    }

    public override string ToString() => $"Error message: \'{Message}\'";

    public enum Type : int
    {
        Other,
        NotAuthorized
    }
}