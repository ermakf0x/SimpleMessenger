﻿using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using System.Runtime.CompilerServices;

namespace SimpleMessenger.Server.MessageHandlers;

abstract class ServerMessageHandlerBase : IMessageHandler
{
    static readonly Success _cachedSuccessResponse = new();


    public void Process(IMessage message, object? state = null)
    {
        var client = state as ServerClient ?? throw new ArgumentNullException(nameof(state));
        var response = Process(message, client);
        ArgumentNullException.ThrowIfNull(response);
        client.SendAsync(response);
    }
    protected abstract IResponse Process(IMessage message, ServerClient client);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static IResponse Error(Error message) => message;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static IResponse Error(string message, Error.Type code) => new Error(message, code);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static IResponse Error(string message) => new Error(message, Core.Messages.Error.Type.Other);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static IResponse Success() => _cachedSuccessResponse;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static IResponse Content(object content) => new JsonContent(content);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static IResponse Content(JsonContent content) => content;
}
