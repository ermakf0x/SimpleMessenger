using System;
using System.IO;

namespace SimpleMessenger.Core.Messages;

public class Error : IResponse
{
    public MessageType MessageType => MessageType.Error;
    public Type Code { get; set; }
    public string Message { get; set; }

    internal Error() { }
    public Error(string message, Type code = Type.Other)
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