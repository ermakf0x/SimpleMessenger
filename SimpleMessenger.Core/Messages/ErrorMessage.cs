using System;
using System.IO;

namespace SimpleMessenger.Core.Messages;

public class ErrorMessage : IMessage
{
    public MessageType Type => MessageType.Error;
    public ErrorMessageType Code { get; set; }
    public string Message { get; set; }

    internal ErrorMessage() { }
    public ErrorMessage(string message, ErrorMessageType code)
    {
        Code = code;
        Message = message ?? throw new ArgumentNullException(nameof(message));
    }

    public void Read(Stream stream)
    {
        Code = stream.Read<ErrorMessageType>();
        Message = stream.ReadString();
    }

    public void Write(Stream stream)
    {
        stream.Write(Code);
        stream.Write(Message);
    }

    public override string ToString() => $"Error message: \'{Message}\'";

}

public enum ErrorMessageType : int
{
    Other,
    NotAuthorized
}
