using SimpleMessenger.Core;
using SimpleMessenger.Core.Messages;
using SimpleMessenger.Server.Model;
using System.Runtime.CompilerServices;

namespace SimpleMessenger.Server.MessageHandlers;

/// <summary>
/// Оброботчик сообщений с проверкой пользователя на авторизацию
/// </summary>
abstract class ServerMessageHandler<TMsg> : ServerMessageHandlerBaseMethods, IMessageHandler
    where TMsg : IMessage
{
    void IMessageHandler.Process(IMessage message, object? state)
    {
        var client = state as ClientHandler ?? throw new ArgumentNullException(nameof(state));
        IResponse response;
        if (IsAuth(message, client))
        {
            response = Process((TMsg)message, client);
            ArgumentNullException.ThrowIfNull(response);
        }
        else response = ErrorMessage.NotAuthorized;

        client.SendAsync(response);
        Logger.Debug($"{client}\r\nMessage: {message.GetType().Name}\r\n{message}\r\nResponse: {response.GetType().Name}\r\n{response}");
    }
    protected abstract IResponse Process(TMsg message, ClientHandler client);

    protected static User2? FindUser(Func<User2, bool> func, User2 self)
    {
        if (func(self)) return self;
        User2? user = self.Contacts.Where(f => func(f.Friend)).FirstOrDefault()?.Friend;

        if (user == null)
        {
            user = LocalDb.GetUser(func);
            if (user == null) return null;
            self.Contacts.Add(new Contact
            {
                Friend = user
            });
            LocalDb.Update(self);
        }

        return user;
    }

    static bool IsAuth(IMessage message, ClientHandler client)
    {
        if(client.CurrentUser is not null) return true;
        if (message is ITokenable msg)
            return LocalDb.GetUserByToken(msg.Token) is not null;
        return false;
    }
}

/// <summary>
/// Оброботчик сообщений без проверки пользователя на авторизацию
/// </summary>
abstract class ServerMessageSlimHandler<TMsg> : ServerMessageHandlerBaseMethods, IMessageHandler
    where TMsg : IMessage
{
    void IMessageHandler.Process(IMessage message, object? state)
    {
        var client = state as ClientHandler ?? throw new ArgumentNullException(nameof(state));
        var response = Process((TMsg)message, client);
        ArgumentNullException.ThrowIfNull(response);
        client.SendAsync(response);
        Logger.Debug($"{client}\r\nMessage: {message.GetType().Name}\r\n{message}\r\nResponse: {response.GetType().Name}\r\n{response}");
    }
    protected abstract IResponse Process(TMsg message, ClientHandler client);
}

abstract class ServerMessageHandlerBaseMethods
{
    static readonly SuccessMessage _cachedSuccessResponse = new();

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
    protected static IResponse Json(object content) => new JsonMessage(content);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static IResponse Json(JsonMessage content) => content;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static IResponse Success() => _cachedSuccessResponse;
}