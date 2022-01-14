namespace SimpleMessenger.Server;

class ServerException : Exception
{
    public ServerException() : base() { }
    public ServerException(string? message) : base(message) { }
    public ServerException(string? message, Exception? innerException) : base(message, innerException) { }
}

class UserNameIsTakenException : ServerException
{
    public UserNameIsTakenException(string userName) : base($"Имя пользователя '{userName}' занято.") { }
}