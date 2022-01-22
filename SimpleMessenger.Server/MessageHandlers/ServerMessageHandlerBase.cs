using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using System.Runtime.CompilerServices;

namespace SimpleMessenger.Server.MessageHandlers;

abstract class ServerMessageHandlerBase<TMsg> : IMessageHandler
    where TMsg : IMessage
{
    static readonly SuccessMessage _cachedSuccessResponse = new();


    void IMessageHandler.Process(IMessage message, object? state)
    {
        var client = state as ClientHandler ?? throw new ArgumentNullException(nameof(state));
        var response = Process((TMsg)message, client);

        #region Debug
        Console.WriteLine($"[SERVER] {client} Message:  {message.GetType().Name}\r\n{message}");
        Console.WriteLine($"[SERVER] {client} Response: {response.GetType().Name}\r\n{response}");
        #endregion

        ArgumentNullException.ThrowIfNull(response);
        client.SendAsync(response);
    }
    protected abstract IResponse Process(TMsg message, ClientHandler client);


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static IResponse Error(ErrorMessage message) => message;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static IResponse Error(string message) => new ErrorMessage(message, ErrorMessage.Type.Other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static IResponse Error(Exception exception) => new ErrorMessage(exception.ToString(), ErrorMessage.Type.Other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static IResponse Error(string message, ErrorMessage.Type code) => new ErrorMessage(message, code);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static IResponse Error(Exception exception, ErrorMessage.Type code) => new ErrorMessage(exception.ToString(), code);

    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static IResponse Success() => _cachedSuccessResponse;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static IResponse Json(object content) => new JsonMessage(content);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static IResponse Json(JsonMessage content) => content;
}
