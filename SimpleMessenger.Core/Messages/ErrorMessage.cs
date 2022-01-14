﻿namespace SimpleMessenger.Core.Messages;

public sealed class ErrorMessage : IMessage, IResponse
{
    public MessageType MessageType => MessageType.Error;
    public Type Code { get; private set; }
    public string Message { get; private set; }

    internal ErrorMessage() { }
    public ErrorMessage(string message, Type code = Type.Other)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Code = code;
    }

    void IMessage.Write(DataWriter writer)
    {
        writer.Write(Code);
        writer.Write(Message);
    }
    void IMessage.Read(DataReader reader)
    {
        Code = reader.Read<Type>();
        Message = reader.ReadString();
    }

    public override string ToString() => $"Error message: \'{Message}\'";

    public enum Type : int
    {
        Other,
        NotAuthorized
    }
}