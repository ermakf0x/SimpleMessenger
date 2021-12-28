using System;
using System.IO;
using System.Text;

namespace SimpleMessenger.Core.Messages;

public class ErrorMessage : IMessage
{
    public MessageType Type => MessageType.Error;
    public ErrorMessageType Code { get; set; }
    public string Message { get; set; }

    public ErrorMessage()
    {
        Message = string.Empty;
    }
    public ErrorMessage(string message, ErrorMessageType code)
    {
        Code = code;
        Message = message ?? throw new ArgumentNullException(nameof(message));
    }

    public void Read(Stream stream)
    {
        var buf = new byte[sizeof(int)];
        stream.Read(buf, 0, buf.Length);
        Code = (ErrorMessageType)BitConverter.ToInt32(buf, 0);
        var reader = new StreamReader(stream, Encoding.UTF8);
        Message = reader.ReadToEnd();
    }

    public void Write(Stream stream)
    {
        stream.Write(BitConverter.GetBytes((int)Code));
        stream.Write(Encoding.UTF8.GetBytes(Message));
    }

    public override string ToString() => $"Error message: \'{Message}\'";

}

public enum ErrorMessageType : int
{
    Other,
    NotAuthorized
}
